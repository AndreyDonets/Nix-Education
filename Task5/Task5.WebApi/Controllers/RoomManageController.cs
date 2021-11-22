using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task5.BLL.DTO;
using Task5.BLL.Interfaces;
using Task5.WebApi.ViewModels.Room;

namespace Task5.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Moderator,Admin")]
    public class RoomManageController : ControllerBase
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
        public IEnumerable<RoomViewModel> RoomList()
        {
            var rooms = roomService.GetAll();
            var categories = categoryService.GetAll();
            var categoryDate = categoryDateService.GetAll().Where(x => x.StartDate.Date <= DateTime.Now.Date).OrderBy(x => x.StartDate);

            var response = new List<RoomViewModel>();
            foreach (var room in rooms)
            {
                response.Add(new RoomViewModel
                {
                    Number = room.Number,
                    Category = categories.FirstOrDefault(x => x.Id == room.CategoryId).Name,
                    Price = categoryDate.LastOrDefault(x => x.CategoryId == room.CategoryId).Price
                });
            }
            return response;
        }

        [HttpGet]
        public ActionResult<RoomViewModel> Get(int number)
        {
            var room = roomService.GetRoomByNumber(number);
            if (room == null)
                return BadRequest();

            var category = categoryService.Get(room.CategoryId);
            var price = categoryDateService.GetCategoryDatesByCategoryId(room.CategoryId).OrderBy(x => x.StartDate)
                .LastOrDefault(x => x.StartDate.Date <= DateTime.Now.Date).Price;

            return new RoomViewModel
            {
                Number = room.Number,
                Category = category.Name,
                Price = price
            };
        }

        [HttpPost]
        public async Task<ActionResult<RoomViewModel>> Add(AddRoomViewModel request)
        {
            if (roomService.GetRoomByNumber(request.Number) != null)
                ModelState.AddModelError("Number", $"Number {request.Number} already exists");

            var category = categoryService.GetCategoryByName(request.Category);
            if (category == null)
                ModelState.AddModelError("Category", $"Category {request.Category} does not exist");

            if (!ModelState.IsValid)
                return BadRequest();

            var room = new RoomDTO { Number = request.Number, CategoryId = category.Id };

            await roomService.CreateAsync(room);

            var price = categoryDateService.GetCategoryDatesByCategoryId(category.Id)
                .OrderBy(x => x.StartDate).LastOrDefault(x => x.StartDate.Date <= DateTime.Now.Date).Price;

            var result = new RoomViewModel()
            {
                Number = request.Number,
                Category = request.Category,
                Price = price
            };

            return result;
        }

        [HttpPut]
        public async Task<ActionResult<RoomViewModel>> Edit(ChangeRoomViewModel request)
        {
            var room = roomService.GetRoomByNumber(request.Number);
            var category = categoryService.GetCategoryByName(request.NewCategory);
            if (category == null && room != null)
                category = categoryService.Get(room.CategoryId);

            if (room == null && request.NewNumber < 0 && roomService.GetRoomByNumber(request.NewNumber) != null)
                return BadRequest();

            var number = request.NewNumber == 0 ? room.Number : request.NewNumber;

            room.CategoryId =  category.Id;
            room.Number = number;

            await roomService.UpdateAsync(room);

            var price = categoryDateService.GetCategoryDatesByCategoryId(category.Id)
                .OrderBy(x => x.StartDate).LastOrDefault(x => x.StartDate.Date <= DateTime.Now.Date).Price;


            var result = new RoomViewModel()
            {
                Number = room.Number,
                Category = category.Name,
                Price = price
            };

            return result;
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int number)
        {
            var room = roomService.GetRoomByNumber(number);

            if (room == null)
                return NotFound();

            await roomService.DeleteAsync(room.Id);

            return Ok();
        }
    }
}
