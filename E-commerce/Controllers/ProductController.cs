using E_commerce.Data;
using E_commerce.Models;
using E_commerce.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        //private ApplicationDbContext _dbContext = new ApplicationDbContext();

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            this._productRepository = productRepository;
            this._categoryRepository = categoryRepository;
        }

        public IActionResult Index(string? query = null, int pageNumber = 1)
        {
            var products = _productRepository.Get(includeProps: [e => e.Category]);
            if (query != null)
            {
                query = query.Trim();
                products = products.Where(e => e.Name.Contains(query) || e.Description.Contains(query));
            }

            products = products.Skip((pageNumber - 1) * 3).Take(3);

            //var products = _dbContext.Products.Include(e=>e.Category).ToList();
            ViewData["message"] = Request.Cookies["message"];
            return View(products.ToList());  
        }

        public IActionResult Create()
        {
            var categories = _categoryRepository.Get().ToList();
            ViewData["categories"] = categories;

            return View(new Product());
        }

        [HttpPost]
        public IActionResult Create(Product product, IFormFile? file)
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

                _productRepository.Create(product);

                //Response.Cookies.Append("message", "Add product successfuly");
                TempData["message"] = "Add product successfuly";

                return RedirectToAction("Index");
            }

            var categories = _categoryRepository.Get().ToList();
            ViewData["categories"] = categories;
            return View(product);
        }

        public IActionResult Edit(int productId)
        {
            var product = _productRepository.GetOne(e => e.Id == productId);

            if (product == null) return RedirectToAction("NotFoundPage", "Home");

            var categories = _categoryRepository.Get().ToList();

            ViewData["categories"] = categories;

            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product, IFormFile? file)
        {
            ModelState.Remove("Img");
            ModelState.Remove("Category");

            var oldProduct = _productRepository.GetOne(e => e.Id == product.Id, tracked: false);

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

                _productRepository.Alter(product);

                TempData["message"] = "Update product successfuly";

                return RedirectToAction("Index");
            }

            product.Img = oldProduct.Img;
            var categories = _categoryRepository.Get().ToList();
            ViewData["categories"] = categories;
            return View(product);
        }

        public IActionResult Delete(int productId)
        {
            var product = _productRepository.GetOne(e=>e.Id == productId);

            if (product == null) return RedirectToAction("NotFoundPage", "Home");

            // Delete old img
            var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", product.Img);
            if (System.IO.File.Exists(oldPath))
            {
                System.IO.File.Delete(oldPath);
            }

            _productRepository.Delete(product);

            TempData["message"] = "Delete product successfuly";

            return RedirectToAction("Index");
        }
    }
}
