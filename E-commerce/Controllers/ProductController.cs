using E_commerce.Data;
using E_commerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();

        public IActionResult Index()
        {
            var products = _dbContext.Products.Include(e=>e.Category).ToList();

            return View(products);
        }

        public IActionResult Create()
        {
            var categories = _dbContext.Categories.ToList();
            ViewBag.Categories = categories;

            return View(categories);
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int productId)
        {
            var product = _dbContext.Products.Find(productId);
            
            if (product == null) return RedirectToAction("NotFoundPage", "Home");

            var categories = _dbContext.Categories.ToList();
            ViewData["categories"] = categories;

            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            _dbContext.Products.Update(product);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
