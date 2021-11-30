using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task5.BLL.DTO;
using Task5.BLL.Interfaces;
using Task7.WebApplication.Models.Book;
using Task7.WebApplication.Models.Room;

namespace Task7.WebApplication.Controllers
{
    public class RoomController : Controller
    {
        private readonly IRoomService roomService;
        private readonly ICategoryService categoryService;
        private readonly ICategoryDateService categoryDateService;
        private readonly IGuestService guestService;
        private readonly IStayService stayService;

        public RoomController(
            IRoomService roomService,
            ICategoryService categoryService,
            ICategoryDateService categoryDateService,
            IGuestService guestService,
            IStayService stayService)
        {
            this.roomService = roomService;
            this.categoryService = categoryService;
            this.categoryDateService = categoryDateService;
            this.guestService = guestService;
            this.stayService = stayService;
        }

        [HttpGet]
        public IActionResult GetAll(DateTime? startDate, DateTime? endDate)
        {
            var model = new List<RoomViewModel>();

            var rooms = roomService.GetAll().ToList();

            var categories = categoryService.GetAll().ToList();

            var categoryDates = categoryDateService.GetAll().Where(x => !x.EndDate.HasValue || x.EndDate > DateTime.Now).OrderBy(x => x.StartDate);

            ViewBag.StartDate = "yyyy-MM-dd";
            ViewBag.EndDate = "yyyy-MM-dd";
            if (startDate.HasValue && startDate.Value.Date >= DateTime.Now.Date)
            {
                var occupiedRooms = new List<StayDTO>();
                    
                if (endDate.HasValue && endDate.Value >= startDate.Value)
                {
                    occupiedRooms = stayService.GetAll()
                        .Where(x => endDate.Value.Date < x.StartDate && x.EndDate >= startDate.Value.Date)
                        .ToList();
                    ViewBag.EndDate = endDate.Value.ToString("yyyy-MM-dd");
                }
                else
                    occupiedRooms.AddRange(stayService.GetAll().Where(x => startDate.Value.Date >= x.StartDate.Date && startDate.Value.Date <= x.EndDate.Date));

                categoryDates = categoryDates.Where(x => x.StartDate.Date <= startDate.Value.Date && (!x.EndDate.HasValue || x.EndDate.Value.Date > startDate.Value.Date)).OrderBy(x => x.StartDate);
                rooms = rooms.Where(x => !occupiedRooms.Select(x => x.RoomId).Any(y => y == x.Id)).ToList();
                ViewBag.StartDate = startDate.Value.ToString("yyyy-MM-dd");
            }

            for (int i = 0; i < rooms.Count(); i++)
            {
                model.Add(new RoomViewModel()
                {
                    Number = rooms[i].Number,
                    Category = categories.FirstOrDefault(x => x.Id == rooms[i].CategoryId).Name,
                    Price = categoryDates.FirstOrDefault(x => x.CategoryId == rooms[i].CategoryId).Price
                });
            }

            return View(model.OrderBy(x => x.Number));
        }

        [HttpGet]
        public IActionResult Book(int number, DateTime? startDate, DateTime? endDate)
        {
            var room = new BookRoomViewModel 
            { 
                Number = number, 
                StartDate = startDate.HasValue ? startDate.Value.Date : DateTime.Now.Date, 
                EndDate = endDate.HasValue ? endDate.Value.Date : DateTime.Now.Date.AddDays(1), 
                BirthDate = new DateTime(2000, 1, 1) 
            };
            return View(room);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(BookRoomViewModel model)
        {

            var room = roomService.GetRoomByNumber(model.Number);
            if (room == null)
            {
                return NotFound();
            }

            var IsOccupied = stayService.GetStaysByRoomNumber(model.Number)
                .Any(x => (model.StartDate >= x.StartDate && model.StartDate < x.EndDate) || (model.EndDate >= x.StartDate && model.EndDate < x.EndDate));

            if (IsOccupied)
                ModelState.AddModelError("Number", "Room is occupied on this date");

            if (ModelState.IsValid)
            {
                var categories = categoryService.GetAll().FirstOrDefault(x => x.Id == room.CategoryId).Name;
                var price = categoryDateService.GetCategoryDatesByCategoryId(room.CategoryId)
                    .Where(x => x.StartDate.Date <= DateTime.Now.Date).OrderBy(x => x.StartDate).LastOrDefault().Price;


                var guest = guestService.GetGuestByPassport(model.Passport);
                if (guest == null)
                {
                    guest = new GuestDTO
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Patronymic = model.Patronymic,
                        Passport = model.Passport,
                        BirthDate = model.BirthDate
                    };
                    await guestService.CreateAsync(guest);
                }

                await stayService.CreateAsync(new StayDTO
                {
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    GuestId = guest.Id,
                    RoomId = room.Id
                });
                return RedirectToAction("GetAll");

            }
            return View(model);
        }
    }
}
