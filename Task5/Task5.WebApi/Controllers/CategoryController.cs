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
        private readonly IRoomService roomService;
        private readonly ICategoryService categoryService;
        private readonly ICategoryDateService categoryDateService;

        public CategoryController(IRoomService roomService, ICategoryService categoryService, ICategoryDateService categoryDateService)
        {
            this.roomService = roomService;
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
                    Id = category.Id,
                    Name = category.Name,
                    Price = categoryDate.LastOrDefault(x => x.CategoryId == category.Id).Price
                });
            }
            return response;
        }

        [HttpGet]
        public CategoryViewModel Get(Guid id)
        {
            var category = categoryService.Get(id);
            var price = categoryDateService.GetAll().OrderBy(x => x.StartDate).LastOrDefault(x => x.CategoryId == category.Id && x.StartDate.Date <= DateTime.Now.Date).Price;

            return new CategoryViewModel { Id = category.Id, Name = category.Name, Price = price };
        }

        [HttpPost]
        public async Task<ActionResult<CategoryViewModel>> Add(CategoryViewModel request)
        {
            if (categoryService.GetAll().Any(x => x.Name == request.Name))
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
                Id = category.Id,
                Name = category.Name,
                Price = categoryDate.Price
            };

            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<CategoryViewModel>> Edit(ChangeCategoryViewModel request)
        {
            var category = categoryService.GetAll().FirstOrDefault(x => x.Id == request.Id);

            if (!categoryService.GetAll().Any(x => x.Name == request.Name))
                ModelState.AddModelError("Name", $"Category name {request.Name} not found");

            if (request.Price < 0)
                ModelState.AddModelError("Price", $"Price cannot be lower than 0");

            if (request.StartDate.HasValue && request.StartDate.Value <= DateTime.Now.Date)
                ModelState.AddModelError("StartDate", $"Start date cannot be earlier than today");

            if (!ModelState.IsValid)
                return BadRequest();

            if (!string.IsNullOrWhiteSpace(request.Name) && category.Name != request.Name)
            {
                category.Name = request.Name;
                await categoryService.UpdateAsync(category);
            }

            var startDate = request.StartDate.HasValue ? request.StartDate.Value : DateTime.Now.Date;

            var cat = categoryDateService.GetAll().FirstOrDefault(x => x.StartDate > startDate);

            if (cat != null)
                await categoryDateService.DeleteAsync(cat.Id);

            var oldCategoryDate = categoryDateService.GetAll().FirstOrDefault(x => x.CategoryId == request.Id && !x.EndDate.HasValue);

            var result = new CategoryViewModel()
            {
                Id = category.Id,
                Name = category.Name,
                Price = oldCategoryDate.Price
            };

            if (oldCategoryDate.Price != 0 && oldCategoryDate.Price != request.Price)
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

            return Ok(result);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(string name)
        {
            var category = categoryService.GetAll().FirstOrDefault(x => x.Name == name);

            if (category == null)
                return NotFound();

            await categoryService.DeleteAsync(category.Id);

            return Ok();
        }
    }
}
