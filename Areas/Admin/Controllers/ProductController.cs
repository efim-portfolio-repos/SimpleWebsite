using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SimpleWebsite.Models;
using SimpleWebsite.Models.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace SimpleWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admins")]
    public class ProductController : Controller
    {
        private IRepository<Product> _products;

        public ProductController(IRepository<Product> products)
        {
            _products = products;
        }

        public ViewResult Index() => View(_products.Entities.Include(p => p.Category));

        public ViewResult Add() => View();

        public IActionResult Edit(int id)
        {
            Product product = _products.Entities.Include(p => p.Category).FirstOrDefault(e => e.Id == id);
            if (product != null)
            {
                return View(product);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _products.Update(product);
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        [HttpPost]
        public IActionResult Add(Product product)
        {
            if (ModelState.IsValid)
            {
                _products.Add(product);
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _products.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}