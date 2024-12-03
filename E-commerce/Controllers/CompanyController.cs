using E_commerce.Data;
using E_commerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers
{
    public class CompanyController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();

        public IActionResult Index()
        {
            return View(context.Companies.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Company company)
        {
            if(ModelState.IsValid)
            {
                context.Companies.Add(company);
                context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(company);
        }

        public IActionResult Edit(int id)
        {
            return View(context.Companies.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Company company)
        {
            if (ModelState.IsValid)
            {
                context.Companies.Update(company);
                context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(company);
        }
    }
}
