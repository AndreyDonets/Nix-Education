using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task5.BLL.DTO;
using Task5.BLL.Interfaces;
using Task7.WebApplication.Models.Room;

namespace Task7.WebApplication.Controllers
{
    [Authorize(Roles = "Moderator,Admin")]
    public class RoomManageController : Controller
    {
        private readonly IRoomService roomService;
        private readonly ICategoryService categoryService;
        private readonly ICategoryDateService categoryDateService;

        public RoomManageController(IRoomService roomService, ICategoryService categoryService, ICategoryDateService categoryDateService)
        {
            this.roomService = roomService;
            this.categoryService = categoryService;
            this.categoryDateService = categoryDateService;
        }

        [HttpGet]
        public IActionResult GetAll(int? number)
        {
            var rooms = roomService.GetAll();
            var categories = categoryService.GetAll();
            var categoryDate = categoryDateService.GetAll().Where(x => x.StartDate.Date <= DateTime.Now.Date).OrderBy(x => x.StartDate);

            var model = new List<RoomViewModel>();
            foreach (var room in rooms)
            {
                model.Add(new RoomViewModel
                {
                    Number = room.Number,
                    Category = categories.FirstOrDefault(x => x.Id == room.CategoryId).Name,
                    Price = categoryDate.LastOrDefault(x => x.CategoryId == room.CategoryId).Price
                });
            }
            if (number.HasValue && number.Value > 0)
                return View(model.FirstOrDefault(x => x.Number == number));
            return View(model.OrderBy(x => x.Number));
        }

        public IActionResult Create()
        {
            var categories = categoryService.GetAll().Select(x => x.Name).OrderBy(x => x);
            ViewBag.Categories = new SelectList(categories);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoomViewModel model)
        {
            if (roomService.GetRoomByNumber(model.Number) != null)
                ModelState.AddModelError("Number", $"Number {model.Number} already exists");

            var category = categoryService.GetCategoryByName(model.Category);

            if (category == null)
                ModelState.AddModelError("Category", $"Category {model.Category} does not exist");

            if (ModelState.IsValid)
            {
                var room = new RoomDTO { Number = model.Number, CategoryId = category.Id };

                await roomService.CreateAsync(room);

                return RedirectToAction("GetAll");
            }
            var categories = categoryService.GetAll().Select(x => x.Name).OrderBy(x => x);
            ViewBag.Categories = new SelectList(categories);
            return View(model);
        }

        public IActionResult Edit(int number)
        {
            var categories = categoryService.GetAll();
            ViewBag.Categories = new SelectList(categories.Select(x => x.Name).OrderBy(x => x), categories.FirstOrDefault(x => x.Id == roomService.GetRoomByNumber(number).CategoryId).Name);
            var model = new ChangeRoomViewModel { Number = number };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ChangeRoomViewModel model)
        {
            var room = roomService.GetRoomByNumber(model.Number);
            if (room == null)
                ModelState.AddModelError("Number", $"Room {model.Number} does not exist");

            var category = categoryService.GetCategoryByName(model.NewCategory);
            if (category == null && room != null)
                category = categoryService.Get(room.CategoryId);

            if (model.NewNumber != room.Number && roomService.GetRoomByNumber(model.NewNumber) != null)
                ModelState.AddModelError("NewNumber", $"Room with number {model.Number} allready exists");

            if (ModelState.IsValid)
            {
                var number = model.NewNumber == 0 ? room.Number : model.NewNumber;
                room.CategoryId = category.Id;
                room.Number = number;

                await roomService.UpdateAsync(room);
                return RedirectToAction("GetAll");
            }

            var categories = categoryService.GetAll();
            ViewBag.Categories = new SelectList(categories.Select(x => x.Name).OrderBy(x => x), categories.FirstOrDefault(x => x.Id == roomService.GetRoomByNumber(model.Number).CategoryId).Name);
            return View(model);
        }

        public async Task<ActionResult> Delete(int number)
        {
            var room = roomService.GetRoomByNumber(number);

            if (room == null)
                return NotFound();

            await roomService.DeleteAsync(room.Id);

            return RedirectToAction("GetAll");
        }
    }
}
