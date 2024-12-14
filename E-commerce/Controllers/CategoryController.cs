using E_commerce.Data;
using E_commerce.Models;
using E_commerce.Repository;
using E_commerce.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        //private ApplicationDbContext dbContext = new ApplicationDbContext();

        //CategoryRepositroy categoryRepositroy = new CategoryRepositroy();

        ICategoryRepository categoryRepositroy;// = new CategoryRepositroy();

        public CategoryController(ICategoryRepository categoryRepositroy)
        {
            this.categoryRepositroy = categoryRepositroy;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            //var categories = dbContext.Categories.Include(e=>e.Products).ToList();
            var categories = categoryRepositroy.Get(includeProps: [e => e.Products]).ToList();
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
            return View(new Category());
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if(ModelState.IsValid)
            {
                categoryRepositroy.Create(category);

                TempData["success"] = "Add Category successfuly";

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        public IActionResult Edit(int categoryId)
        {
            //var category = dbContext.Categories.Find(categoryId);
            var category = categoryRepositroy.GetOne(e => e.Id == categoryId);

            if (category != null)
            {
                return View(category);
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if(ModelState.IsValid)
            {
                categoryRepositroy.Alter(category);

                //var category = dbContext.Categories.Find(id);
                //category.Name = name;

                TempData["success"] = "Update Category successfuly";

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        public IActionResult Delete(int categoryId)
        {
            //var category = dbContext.Categories.Find(categoryId);
            var category = categoryRepositroy.GetOne(e => e.Id == categoryId);
            if(category != null)
            {
                categoryRepositroy.Delete(category);

                TempData["success"] = "Delete Category successfuly";

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
    }
}
