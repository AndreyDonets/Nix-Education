using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task5.BLL.Interfaces;
using Task7.WebApplication.Models.Book;

namespace Task7.WebApplication.Controllers
{
    [Authorize(Roles = "Moderator,Admin")]
    public class ManageController : Controller
    {
        private readonly IRoomService roomService;
        private readonly ICategoryDateService categoryDateService;
        private readonly IStayService stayService;
        private readonly IGuestService guestService;

        public ManageController(IRoomService roomService, ICategoryDateService categoryDateService, IStayService stayService, IGuestService guestService)
        {
            this.roomService = roomService;
            this.categoryDateService = categoryDateService;
            this.stayService = stayService;
            this.guestService = guestService;
        }

        [HttpGet]
        public IActionResult Bookings(DateTime? date)
        {
            var stays = stayService.GetAll().Where(x => x.EndDate.Date >= DateTime.Now.Date);
            if (stays.Count() == 0)
                return View();

            if (date.HasValue && date.Value.Date >= DateTime.Now)
            {
                stays = stays.Where(x => x.StartDate.Date <= date.Value.Date && x.EndDate > date.Value.Date);
                ViewBag.Date = date.Value.ToString("yyyy-MM-dd");
            }

            var rooms = roomService.GetAll().Where(x => stays.Any(y => y.RoomId == x.Id));

            var guests = guestService.GetAll().Where(x => stays.Any(y => y.GuestId == x.Id));

            var model = new List<StayViewModel>();
            foreach (var stay in stays)
            {
                model.Add(new StayViewModel
                {
                    StartDate = stay.StartDate,
                    EndDate = stay.EndDate,
                    CheckedIn = stay.CheckedIn,
                    CheckedOut = stay.CheckedOut,
                    Passport = guests.FirstOrDefault(x => x.Id == stay.GuestId).Passport,
                    RoomNumber = rooms.FirstOrDefault(x => x.Id == stay.RoomId).Number
                });
            }

            return View(model.OrderBy(x => x.StartDate));
        }

        public async Task<IActionResult> CheckIn(int number)
        {
            var room = roomService.GetRoomByNumber(number);
            if (room == null)
                return NotFound();

            var stay = stayService.GetStaysByRoomId(room.Id).FirstOrDefault(x => !x.CheckedIn && x.StartDate.Date <= DateTime.Now.Date && x.EndDate.Date >= DateTime.Now.Date);
            if (stay == null)
                return NotFound();

            stay.CheckedIn = true;

            await stayService.UpdateAsync(stay);

            return RedirectToAction("Bookings");
        }

        public async Task<IActionResult> CheckOut(int number)
        {
            var room = roomService.GetRoomByNumber(number);
            if (room == null)
                return NotFound();

            var stay = stayService.GetStaysByRoomId(room.Id).FirstOrDefault(x => x.CheckedIn && !x.CheckedOut && x.StartDate.Date <= DateTime.Now.Date && x.EndDate.Date >= DateTime.Now.Date);
            if (stay == null)
                return NotFound();

            stay.CheckedOut = true;

            await stayService.UpdateAsync(stay);

            return RedirectToAction("Bookings");
        }

        [HttpGet]
        public IActionResult Profit(DateTime? startDate, DateTime? endDate)
        {

            var start = startDate.HasValue ? startDate.Value : DateTime.Now.Date;
            var end = endDate.HasValue ? endDate.Value : DateTime.Now.Date;

            ViewBag.StartDate = start.ToString("yyyy-MM-dd");
            ViewBag.EndDate = end.ToString("yyyy-MM-dd");

            if (start >= end || end > DateTime.Now.Date)
                return View();

            var stays = stayService.GetAll().Where(x => end.Date > x.StartDate.Date && x.EndDate.Date >= start.Date);

            var rooms = roomService.GetAll().Where(x => stays.Any(y => y.RoomId == x.Id));

            var categoryDates = categoryDateService.GetAll()
                .Where(x => rooms.Any(y => y.CategoryId == x.CategoryId) && x.StartDate.Date < end.Date && (!x.EndDate.HasValue || x.EndDate.Value.Date >= start.Date));

            var result = new decimal();
            foreach (var stay in stays)
            {
                var calcStartDay = stay.StartDate > start ? stay.StartDate : start;

                var calcEndDay = stay.EndDate > end ? end : stay.EndDate;

                var days = (calcEndDay.Date - calcStartDay.Date).Days;

                var room = rooms.FirstOrDefault(x => x.Id == stay.RoomId);

                var date = start;
                while (true)
                {
                    var catDates = categoryDates.FirstOrDefault(x => x.CategoryId == room.CategoryId && x.StartDate.Date <= date.Date && (!x.EndDate.HasValue || x.EndDate.Value > date));
                    if (catDates.EndDate.HasValue)
                    {
                        var calcDays = (catDates.EndDate.Value.Date - date.Date).Days;

                        if (calcDays < days)
                        {
                            result += calcDays * catDates.Price;
                            days -= calcDays;
                            date = catDates.EndDate.Value;
                            continue;
                        }
                    }
                    result += days * catDates.Price;
                    break;
                }
            }
            return View(result);
        }
    }
}