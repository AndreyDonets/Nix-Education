using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task5.BLL.Interfaces;
using Task5.WebApi.ViewModels.Book;

namespace Task5.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Moderator,Admin")]
    public class ManageController : ControllerBase
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
        public ActionResult<IEnumerable<StayViewModel>> ListBook()
        {
            var stays = stayService.GetAll().Where(x => x.EndDate.Date > DateTime.Now.Date && !x.CheckedIn);
            if (stays.Count() == 0)
                return null;

            var rooms = roomService.GetAll().Where(x => stays.Any(y => y.RoomId == x.Id));

            var guests = guestService.GetAll().Where(x => stays.Any(y => y.GuestId == x.Id));

            var result = new List<StayViewModel>();
            foreach (var stay in stays)
            {
                result.Add(new StayViewModel
                {
                    StartDate = stay.StartDate,
                    EndDate = stay.EndDate,
                    CheckedIn = stay.CheckedIn,
                    CheckedOut = stay.CheckedOut,
                    Passport = guests.FirstOrDefault(x => x.Id == stay.GuestId).Passport,
                    RoomNumber = rooms.FirstOrDefault(x => x.Id == stay.RoomId).Number
                });
            }

            return result;
        }

        [HttpGet]
        public ActionResult<IEnumerable<StayViewModel>> GetBookingsByDate(DateTime date)
        {
            var stays = stayService.GetAll().Where(x => x.StartDate.Date <= date.Date && x.EndDate > date);
            if (stays == null)
                return BadRequest();

            var rooms = roomService.GetAll().Where(x => stays.Any(y => y.RoomId == x.Id));

            var guests = guestService.GetAll().Where(x => stays.Any(y => y.GuestId == x.Id));

            var result = new List<StayViewModel>();
            foreach (var stay in stays)
            {
                result.Add(new StayViewModel
                {
                    StartDate = stay.StartDate,
                    EndDate = stay.EndDate,
                    CheckedIn = stay.CheckedIn,
                    CheckedOut = stay.CheckedOut,
                    Passport = guests.FirstOrDefault(x => x.Id == stay.GuestId).Passport,
                    RoomNumber = rooms.FirstOrDefault(x => x.Id == stay.RoomId).Number
                });
            }

            return result;
        }

        [HttpPut]
        public async Task<ActionResult> CheckIn(int number)
        {
            var room = roomService.GetAll().FirstOrDefault(x => x.Number == number);
            if (room == null)
                return BadRequest();

            var stay = stayService.GetAll().FirstOrDefault(x => x.RoomId == room.Id && !x.CheckedIn && x.StartDate.Date <= DateTime.Now.Date && x.EndDate.Date > DateTime.Now.Date);
            if (stay == null)
                return BadRequest();

            stay.CheckedIn = true;

            await stayService.UpdateAsync(stay);

            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> CheckOut(int number)
        {
            var room = roomService.GetAll().FirstOrDefault(x => x.Number == number);
            if (room == null)
                return BadRequest();

            var stay = stayService.GetAll().FirstOrDefault(x => x.RoomId == room.Id && x.CheckedIn && !x.CheckedOut && x.StartDate.Date <= DateTime.Now.Date && x.EndDate.Date >= DateTime.Now.Date);
            if (stay == null)
                return BadRequest();

            if (stay.EndDate.Date > DateTime.Now.Date)
                stay.EndDate = DateTime.Now.Date;

            stay.CheckedOut = true;

            await stayService.UpdateAsync(stay);

            return Ok();
        }

        [HttpGet]
        public ActionResult<decimal> Profit(DateTime startDate, DateTime endDate)
        {
            if (startDate < endDate || endDate > DateTime.Now.Date)
                return BadRequest();

            var stays = stayService.GetAll().Where(x => endDate.Date > x.StartDate.Date && x.EndDate.Date >= startDate.Date);

            var rooms = roomService.GetAll().Where(x => stays.Any(y => y.RoomId == x.Id));

            var categoryDates = categoryDateService.GetAll()
                .Where(x => rooms.Any(y => y.CategoryId == x.CategoryId) && x.StartDate.Date < endDate.Date && (!x.EndDate.HasValue || x.EndDate.Value.Date >= startDate.Date));

            var result = new decimal();
            foreach (var stay in stays)
            {
                var calcStartDay = stay.StartDate > startDate ? stay.StartDate : startDate;

                var calcEndDay = stay.EndDate > endDate ? endDate : stay.EndDate;

                var days = (calcEndDay.Date - calcStartDay.Date).Days;

                var room = rooms.FirstOrDefault(x => x.Id == stay.RoomId);

                var date = startDate;
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
            return Ok(result);
        }
    }
}