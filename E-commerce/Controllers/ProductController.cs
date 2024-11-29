using E_commerce.Data;
using E_commerce.Models;
using Microsoft.AspNetCore.Http;
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
            ViewData["message"] = Request.Cookies["message"];
            return View(products);
        }

        public IActionResult Create()
        {
            var categories = _dbContext.Categories.ToList();
            ViewData["categories"] = categories;

            return View(new Product());
        }

        [HttpPost]
        public IActionResult Create(Product product, IFormFile file)
        {
            ModelState.Remove("Img");
            ModelState.Remove("Category");

            if(ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    // Genereate name
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                    // Save in wwwroot
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }

                    // Save in db
                    product.Img = fileName;
                }

                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();

                //Response.Cookies.Append("message", "Add product successfuly");
                TempData["message"] = "Add product successfuly";

                return RedirectToAction("Index");
            }

            var categories = _dbContext.Categories.ToList();
            ViewData["categories"] = categories;
            return View(product);
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
        public IActionResult Edit(Product product, IFormFile file)
        {
            ModelState.Remove("Img");
            ModelState.Remove("Category");

            var oldProduct = _dbContext.Products.Where(e => e.Id == product.Id).AsNoTracking().FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    // Genereate name
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                    // Save in wwwroot
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }

                    // Delete old img
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", oldProduct.Img);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }

                    product.Img = fileName;
                }
                else
                {
                    product.Img = oldProduct.Img;
                }

                _dbContext.Products.Update(product);
                _dbContext.SaveChanges();

                TempData["message"] = "Update product successfuly";

                return RedirectToAction("Index");
            }
            product.Img = oldProduct.Img;
            var categories = _dbContext.Categories.ToList();
            ViewData["categories"] = categories;
            return View(product);
        }

        public IActionResult Delete(int productId)
        {
            var product = _dbContext.Products.Find(productId);

            if (product == null) return RedirectToAction("NotFoundPage", "Home");

            // Delete old img
            var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", product.Img);
            if (System.IO.File.Exists(oldPath))
            {
                System.IO.File.Delete(oldPath);
            }

            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();

            TempData["message"] = "Delete product successfuly";

            return RedirectToAction("Index");
        }
    }
}
