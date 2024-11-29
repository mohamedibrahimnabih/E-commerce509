﻿using E_commerce.Data;
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
            return View(new Category());
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if(ModelState.IsValid)
            {
                dbContext.Categories.Add(new()
                {
                    Name = category.Name
                });

                dbContext.SaveChanges();

                TempData["success"] = "Add Category successfuly";

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        public IActionResult Edit(int categoryId)
        {
            var category = dbContext.Categories.Find(categoryId);

            if(category != null)
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
                dbContext.Categories.Update(new Category
                {
                    Name = category.Name,
                    Id = category.Id
                });

                //var category = dbContext.Categories.Find(id);
                //category.Name = name;

                dbContext.SaveChanges();

                TempData["success"] = "Update Category successfuly";

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        public IActionResult Delete(int categoryId)
        {
            var category = dbContext.Categories.Find(categoryId);
            if(category != null)
            {
                dbContext.Categories.Remove(category);
                dbContext.SaveChanges();

                TempData["success"] = "Delete Category successfuly";

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
    }
}
