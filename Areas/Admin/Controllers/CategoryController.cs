using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SimpleWebsite.Models;
using SimpleWebsite.Models.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace SimpleWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admins")]
    public class CategoryController : Controller
    {
        private IRepository<Category> _categories;
        public CategoryController(IRepository<Category> categories)
        {
            _categories = categories;
        }

        public ViewResult Index() => View(_categories.Entities);

        public ViewResult Add() => View();

        [HttpPost]
        public IActionResult Add(Category category) 
        {
            if (ModelState.IsValid)
            {
                _categories.Add(category);
                return RedirectToAction(nameof(Index));
            }
            
            return View(category);
        }

        public IActionResult Edit(int id)
        {
            Category category = _categories.Entities.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                return View(category);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _categories.Update(category);
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _categories.Delete(id);
            return RedirectToAction(nameof(Index));
        }


    }
}