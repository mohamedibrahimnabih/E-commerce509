using System.Diagnostics;
using E_commerce.Data;
using E_commerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _context = new ApplicationDbContext();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        public IActionResult Details(int productId)
        {
            var product = _context.Products.Find(productId);
            //var product = _context.Products.Where(e=>e.Id == productId).FirstOrDefault();
            if (product != null) return View(product);

            return RedirectToAction("NotFoundPage");
        }

        public IActionResult NotFoundPage()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
