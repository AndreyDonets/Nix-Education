using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task5.BLL.DTO;
using Task5.BLL.Interfaces;
using Task7.WebApplication.Models.Categories;

namespace Task7.WebApplication.Controllers
{
    [Authorize(Roles = "Moderator,Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly ICategoryDateService categoryDateService;

        public CategoryController(ICategoryService categoryService, ICategoryDateService categoryDateService)
        {
            this.categoryService = categoryService;
            this.categoryDateService = categoryDateService;
        }

        [HttpGet]
        public IActionResult GetAll(string name)
        {
            var categories = categoryService.GetAll();
            var categoryDate = categoryDateService.GetAll().Where(x => x.StartDate.Date <= DateTime.Now.Date).OrderBy(x => x.StartDate);
            var model = new List<CategoryViewModel>();

            if (!string.IsNullOrWhiteSpace(name))
                categories = categories.Where(x => x.Name == name);

            foreach (var category in categories)
            {
                model.Add(new CategoryViewModel
                {
                    Name = category.Name,
                    Price = categoryDate.LastOrDefault(x => x.CategoryId == category.Id).Price
                });
            }
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            if (categoryService.GetCategoryByName(model.Name) != null)
                ModelState.AddModelError("Name", $"Category name {model.Name} already exists");

            if (model.Price < 0)
                ModelState.AddModelError("Price", $"Price cannot be lower than 0");

            if (ModelState.IsValid)
            {
                var category = new CategoryDTO() { Name = model.Name };
                var categoryDate = new CategoryDateDTO()
                {
                    StartDate = DateTime.Now.Date,
                    Price = model.Price,
                    CategoryId = category.Id
                };

                await categoryService.CreateAsync(category);
                await categoryDateService.CreateAsync(categoryDate);

                return RedirectToAction("GetAll");
            }
            return View(model);
        }

        public IActionResult Edit(string name)
        {
            var category = categoryService.GetCategoryByName(name);
            var categoryDate = categoryDateService.GetCategoryDatesByCategoryId(category.Id).Where(x => x.StartDate.Date <= DateTime.Now.Date).OrderBy(x => x.StartDate).LastOrDefault();
            var model = new ChangeCategoryViewModel { Name = category.Name, Price = categoryDate.Price };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ChangeCategoryViewModel model)
        {
            var category = categoryService.GetCategoryByName(model.Name);

            if (category == null)
                ModelState.AddModelError("Name", $"Category not found");

            if (model.Price < 0)
                ModelState.AddModelError("Price", $"Price cannot be lower than 0");

            if (model.StartDate.HasValue && model.StartDate.Value <= DateTime.Now.Date)
                ModelState.AddModelError("StartDate", $"Start date cannot be earlier than today");

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(model.NewName) && category.Name != model.NewName)
                {
                    category.Name = model.NewName;
                    await categoryService.UpdateAsync(category);
                }

                if (model.Price > 0)
                {
                    var startDate = model.StartDate.HasValue ? model.StartDate.Value.Date : DateTime.Now.Date;

                    var categoryDatePrev = categoryDateService.GetCategoryDatesByCategoryId(category.Id).FirstOrDefault(x => x.StartDate <= startDate && (!x.EndDate.HasValue || x.EndDate > startDate));
                    if (categoryDatePrev != null && categoryDatePrev.StartDate.Date == startDate)
                    {
                        await categoryDateService.DeleteAsync(categoryDatePrev.Id);
                    }
                    else if (categoryDatePrev != null)
                    {
                        categoryDatePrev.EndDate = startDate;
                        await categoryDateService.UpdateAsync(categoryDatePrev);
                    }

                    var categoryDateNext = categoryDateService.GetCategoryDatesByCategoryId(category.Id).OrderBy(x => x.StartDate).FirstOrDefault(x => x.StartDate > startDate);

                    var categoryDate = new CategoryDateDTO()
                    {
                        Price = model.Price,
                        CategoryId = category.Id,
                        StartDate = startDate
                    };

                    if (categoryDateNext != null)
                        categoryDate.EndDate = categoryDateNext.StartDate;

                    await categoryDateService.CreateAsync(categoryDate);
                    return RedirectToAction("GetAll");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(string name)
        {
            var category = categoryService.GetCategoryByName(name);

            if (category == null)
                return NotFound();

            await categoryService.DeleteAsync(category.Id);

            return RedirectToAction("GetAll");
        }
    }
}
