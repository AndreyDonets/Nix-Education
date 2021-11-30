 using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task5.BLL.DTO;
using Task5.BLL.Interfaces;
using Task5.WebApi.ViewModels.Categories;

namespace Task5.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Moderator,Admin")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;
        private readonly ICategoryDateService categoryDateService;

        public CategoryController(ICategoryService categoryService, ICategoryDateService categoryDateService)
        {
            this.categoryService = categoryService;
            this.categoryDateService = categoryDateService;
        }

        [HttpGet]
        public IEnumerable<CategoryViewModel> GetAll()
        {
            var categories = categoryService.GetAll();
            var categoryDate = categoryDateService.GetAll().Where(x => x.StartDate.Date <= DateTime.Now.Date).OrderBy(x => x.StartDate);

            var response = new List<CategoryViewModel>();
            foreach (var category in categories)
            {
                response.Add(new CategoryViewModel
                {
                    Name = category.Name,
                    Price = categoryDate.LastOrDefault(x => x.CategoryId == category.Id).Price
                });
            }
            return response;
        }

        [HttpGet]
        public CategoryViewModel GetByName(string name)
        {
            var category = categoryService.GetCategoryByName(name);
            var price = categoryDateService.GetCategoryDatesByCategoryId(category.Id)
                .OrderBy(x => x.StartDate).LastOrDefault(x => x.StartDate.Date <= DateTime.Now.Date).Price;

            return new CategoryViewModel { Name = category.Name, Price = price };
        }

        [HttpPost]
        public async Task<ActionResult<CategoryViewModel>> Add(CategoryViewModel request)
        {
            if (categoryService.GetCategoryByName(request.Name) != null)
                ModelState.AddModelError("Name", $"Category name {request.Name} already exists");

            if (request.Price < 0)
                ModelState.AddModelError("Price", $"Price cannot be lower than 0");

            if (!ModelState.IsValid)
                return BadRequest();

            var category = new CategoryDTO() { Name = request.Name };
            var categoryDate = new CategoryDateDTO()
            {
                StartDate = DateTime.Now.Date,
                Price = request.Price,
                CategoryId = category.Id
            };

            await categoryService.CreateAsync(category);
            await categoryDateService.CreateAsync(categoryDate);

            var result = new CategoryViewModel()
            {
                Name = category.Name,
                Price = categoryDate.Price
            };

            return result;
        }

        [HttpPut]
        public async Task<ActionResult<CategoryViewModel>> Edit(ChangeCategoryViewModel request)
        {
            var category = categoryService.GetCategoryByName(request.Name);

            if (category == null)
                ModelState.AddModelError("Name", $"Category name {request.Name} not found");

            if (request.Price < 0)
                ModelState.AddModelError("Price", $"Price cannot be lower than 0");

            if (request.StartDate.HasValue && request.StartDate.Value <= DateTime.Now.Date)
                ModelState.AddModelError("StartDate", $"Start date cannot be earlier than today");

            if (!ModelState.IsValid)
                return BadRequest();

            if (!string.IsNullOrWhiteSpace(request.NewName) && category.Name != request.NewName)
            {
                category.Name = request.NewName;
                await categoryService.UpdateAsync(category);
            }

            var startDate = request.StartDate.HasValue ? request.StartDate.Value.Date : DateTime.Now.Date;

            var cat = categoryDateService.GetCategoryDatesByCategoryId(category.Id).FirstOrDefault(x => x.StartDate > startDate);

            if (cat != null)
                await categoryDateService.DeleteAsync(cat.Id);

            var oldCategoryDate = categoryDateService.GetCategoryDatesByCategoryId(category.Id)
                .OrderBy(x => x.StartDate).LastOrDefault(x => x.StartDate < startDate);

            var result = new CategoryViewModel()
            {
                Name = category.Name,
                Price = oldCategoryDate.Price
            };

            if (request.Price != 0 && oldCategoryDate.Price != request.Price)
            {
                result.Price = request.Price;
                var categoryDate = new CategoryDateDTO()
                {
                    Price = request.Price,
                    CategoryId = category.Id,
                    StartDate = startDate
                };

                oldCategoryDate.EndDate = startDate;

                await categoryDateService.UpdateAsync(oldCategoryDate);

                await categoryDateService.CreateAsync(categoryDate);
            }

            return result;
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(string name)
        {
            var category = categoryService.GetCategoryByName(name);

            if (category == null)
                return NotFound();

            await categoryService.DeleteAsync(category.Id);

            return Ok();
        }
    }
}
