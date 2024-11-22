using E_commerce.Data;
using E_commerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Controllers
{
    public class CategoryController : Controller
    {
        private ApplicationDbContext dbContext = new ApplicationDbContext();

        public IActionResult Index()
        {
            var categories = dbContext.Categories.Include(e => e.Products).ToList();
            //List<int> ints = new List<int>();

            //foreach (var item in categories)
            //{
            //    var count = dbContext.Products.Where(e => e.CategoryId == item.Id).Count();
            //    ints.Add(count);
            //}

            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string name)
        {
            dbContext.Categories.Add(new()
            {
                Name = name
            });

            dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        
    }
}
