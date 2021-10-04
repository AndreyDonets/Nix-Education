using System;
using System.Collections.Generic;
using System.Linq;
using Task2.Cmd.DataManager;
using Task2.Cmd.Models;

namespace Task2.Cmd.Controllers
{
    public class HotelRoomController
    {
        private UnitOfWork unit;
        public HotelRoomController(UnitOfWork unitOfWork) => unit = unitOfWork;
        public HotelRoom AddHotelRoom(int number, CategoriesHotelRoom category, decimal price)
        {
            var hotelRooms = unit.HotelRoomRepository.GetList();
            var lastId = 0;
            if (hotelRooms.Count > 0)
                lastId = hotelRooms.Max(x => x.Id);
            var nextId = lastId + 1;
            if (number < 1 && hotelRooms.Any(x => x.Number == number))
                throw new Exception("Number must be unique and greater than zero");
            if (price <= 0)
                throw new ArgumentException("Price must be greater than zero");
            var hotelRoom = new HotelRoom()
            {
                Id = nextId,
                Number = number,
                Category = category,
                Price = price
            };
            unit.HotelRoomRepository.Add(hotelRoom);
            unit.Save();
            return hotelRoom;
        }
        public List<HotelRoom> GetAllHotelRooms() => unit.HotelRoomRepository.GetList();
        public HotelRoom GetByIdHotelRoom(int id) => unit.HotelRoomRepository.GetById(id);
        public HotelRoom GetByNumberHotelRoom(int number) => unit.HotelRoomRepository.GetList().FirstOrDefault(x => x.Number == number);
        public void DeleteHotelRoom(HotelRoom hotelRoom)
        {
            unit.HotelRoomRepository.Delete(hotelRoom);
            unit.Save();
        }
    }
}
