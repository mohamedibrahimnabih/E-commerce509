using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers
{
    public class WelcomeController : Controller
    {
        public IActionResult Index()
        {
            string name = "Mohamed";
            return View(model: name);
        }
    }
}
