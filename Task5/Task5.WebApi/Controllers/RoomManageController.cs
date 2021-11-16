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
            var room = roomService.GetAll().FirstOrDefault(x => x.Number == number);
            if (room == null)
                return BadRequest();

            var category = categoryService.Get(room.CategoryId);
            var price = categoryDateService.GetAll().OrderBy(x => x.StartDate).FirstOrDefault(x => x.CategoryId == category.Id && x.StartDate.Date <= DateTime.Now.Date).Price;

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
            if (roomService.GetAll().Any(x => x.Number == request.Number))
                ModelState.AddModelError("Number", $"Number {request.Number} already exists");

            var categories = await categoryService.GetAllAsync();
            if (!categories.Any(x => x.Name == request.Category))
                ModelState.AddModelError("Category", $"Category {request.Category} does not exist");

            if (!ModelState.IsValid)
                return BadRequest();

            var categoryId = categories.FirstOrDefault(x => x.Name == request.Category).Id;

            var room = new RoomDTO { Number = request.Number, CategoryId = categoryId };

            var price = categoryDateService.GetAll().OrderBy(x => x.StartDate).FirstOrDefault(x => x.CategoryId == categoryId && x.StartDate.Date <= DateTime.Now.Date).Price;

            var result = new RoomViewModel()
            {
                Number = request.Number,
                Category = request.Category,
                Price = price
            };

            await roomService.CreateAsync(room);

            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<RoomViewModel>> Edit(ChangeRoomViewModel request)
        {
            var room = roomService.GetAll().FirstOrDefault(x => x.Number == request.Number);
            var categories = await categoryService.GetAllAsync();

            if (room == null || string.IsNullOrWhiteSpace(request.NewCategory) || !categories.Any(x => x.Name == request.NewCategory))
                return BadRequest();

            if (request.NewNumber < 0)
                ModelState.AddModelError("Number", $"Number cannot be lower than 0");

            var categoryId = categories.FirstOrDefault(x => x.Name == request.NewCategory)?.Id;
            var newNumber = request.NewNumber == 0 ? room.Number : request.NewNumber;

            if (request.Number == newNumber && (string.IsNullOrWhiteSpace(request.NewCategory) || categoryId == null))
                ModelState.AddModelError("Request", $"Data not changed");

            if (!ModelState.IsValid)
                return BadRequest();

            room.CategoryId = categoryId != null ? categoryId.Value : room.CategoryId;
            room.Number = newNumber;

            await roomService.UpdateAsync(room);

            var price = categoryDateService.GetAll().OrderBy(x => x.StartDate).FirstOrDefault(x => x.CategoryId == categoryId && x.StartDate.Date <= DateTime.Now.Date).Price;


            var result = new RoomViewModel()
            {
                Number = room.Number,
                Category = categories.FirstOrDefault(x => x.Id == room.CategoryId).Name,
                Price = price
            };

            return Ok(result);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int number)
        {
            var room = roomService.GetAll().FirstOrDefault(x => x.Number == number);

            if (room == null)
                return NotFound();

            await roomService.DeleteAsync(room.Id);

            return Ok();
        }
    }
}
