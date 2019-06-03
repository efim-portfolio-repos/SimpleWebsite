using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleWebsite.Models;
using SimpleWebsite.Models.Repositories;

namespace SimpleWebsite.Controllers
{
    public class ProductController : Controller
    {
        private IRepository<Category> _categories;
        private IRepository<Product> _products;
        public ProductController(IRepository<Category> categories, IRepository<Product> products)
        {
            _categories = categories;
            _products = products;
        }
        public ViewResult Index() 
        {
            List<Category> categories = _categories.Entities.Include(c => c.Products).OrderBy(c => c.Name).ToList();
            categories.Add(new Category {
                Name = "Без категории",
                Products = _products.Entities.Where(p => p.CategoryId == null).ToList()
            });

            return View(categories);
        }
    }
}