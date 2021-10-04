using System.Collections.Generic;
using System.Linq;
using Task2.Cmd.Interfaces;
using Task2.Cmd.Models;

namespace Task2.Cmd.DataManager.Repositories
{
    public class HotelRoomRepository : IRepository<HotelRoom>
    {
        private Context _context;
        public HotelRoomRepository(Context context) => _context = context;
        public void Add(HotelRoom entity)
        {
            if (GetById(entity.Id) == null)
                _context.HotelRooms.Add(entity);
        }
        public void Delete(HotelRoom entity)
        {
            if (GetById(entity.Id) != null)
                _context.HotelRooms.Remove(entity);
        }
        public HotelRoom GetById(int id) => _context.HotelRooms.FirstOrDefault(x => x.Id == id);
        public List<HotelRoom> GetList() => _context.HotelRooms.ToList();
        public void Update(HotelRoom entity)
        {
            Delete(entity);
            Add(entity);
        }
    }
}
