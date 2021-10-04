using System;
using System.Collections.Generic;
using System.Linq;
using Task2.Cmd.DataManager;
using Task2.Cmd.Models;

namespace Task2.Cmd.Controllers
{
    public class InternalHotelInformationController
    {
        private UnitOfWork unit;
        public InternalHotelInformationController(UnitOfWork unitOfWork) => unit = unitOfWork;
        public InternalHotelInformation ReserveHotelRoom(int visitorId, int hotelRoomId, DateTime reservedDate)
        {
            var HotelInformations = unit.InternalHotelInformation.GetList();
            var lastId = 0;
            if (HotelInformations.Count > 0)
                lastId = HotelInformations.Max(x => x.Id);
            var nextId = lastId + 1;
            if (unit.VisitorRepository.GetById(visitorId) == null)
                throw new Exception("There is no such visitor in the database");
            if (unit.HotelRoomRepository.GetById(hotelRoomId) == null)
                throw new Exception("There is no such hotel room in the database");
            if (reservedDate < DateTime.Now)
                throw new Exception("Can't book a room for the past days");
            var roomReservation = new InternalHotelInformation()
            {
                Id = nextId,
                VisitorId = visitorId,
                HotelRoomId = hotelRoomId,
                ReservedDate = reservedDate
            };
            unit.InternalHotelInformation.Add(roomReservation);
            unit.Save();
            return roomReservation;
        }
        public InternalHotelInformation Registration(int visitorId, int hotelRoomId)
        {
            var HotelInformations = unit.InternalHotelInformation.GetList();
            var lastId = 0;
            if (HotelInformations.Count > 0)
                lastId = HotelInformations.Max(x => x.Id);
            var nextId = lastId + 1;
            if (unit.VisitorRepository.GetById(visitorId) == null)
                throw new Exception("There is no such visitor in the database");
            if (unit.HotelRoomRepository.GetById(hotelRoomId) == null)
                throw new Exception("There is no such hotel room in the database");
            var roomReservation = new InternalHotelInformation()
            {
                Id = nextId,
                VisitorId = visitorId,
                HotelRoomId = hotelRoomId,
                RegistrationDate = DateTime.Now
            };
            unit.InternalHotelInformation.Add(roomReservation);
            unit.Save();
            return roomReservation;
        }
        public InternalHotelInformation Registration(InternalHotelInformation reserve)
        {
            if (reserve == null)
                throw new ArgumentNullException();
            if (reserve.ReservedDate.HasValue)
                throw new Exception("Visitor is already registered");
            if (DateTime.Today != reserve.ReservedDate.Value.Date)
                throw new Exception("The date of the reservation does not coincide with the date of check-in");
            reserve.RegistrationDate = DateTime.Now;
            unit.InternalHotelInformation.Update(reserve);
            unit.Save();
            return reserve;
        }
        public void Eviction(InternalHotelInformation information)
        {
            if (information == null)
                throw new ArgumentNullException();
            if (!information.RegistrationDate.HasValue)
                throw new ArgumentException("The visitor must be registered");
            if (information.CheckOutDate.HasValue)
                throw new ArgumentException("The visitor has already checked out");
            information.CheckOutDate = DateTime.Now;
            unit.InternalHotelInformation.Update(information);
            unit.Save();
        }
        public List<InternalHotelInformation> GetAllHotelInformation() => unit.InternalHotelInformation.GetList();
        public InternalHotelInformation GetByIdHotelInformation(int id) => unit.InternalHotelInformation.GetById(id);
    }
}
