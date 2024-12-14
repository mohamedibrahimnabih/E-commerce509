using E_commerce.Models;
using E_commerce.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers
{
    public class RequestsComapnyController : Controller
    {
        private readonly IRequestCompnayRepository requestCompnayRepo;

        public RequestsComapnyController(IRequestCompnayRepository requestCompnayRepo)
        {
            this.requestCompnayRepo = requestCompnayRepo;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View(requestCompnayRepo.Get().ToList());
        }

        public IActionResult CreateNewRequest()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateNewRequest(RequestCompnay requestCompnay)
        {
            if(ModelState.IsValid)
            {
                requestCompnayRepo.Create(requestCompnay);
                TempData["success"] = "تم اضافه الطلب بنجاح برجاء انتظار 3 ايام";

                return RedirectToAction("Index", "Home");
            }
            return View(requestCompnay);
        }
    }
}
