using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task2.Cmd.DataManager.Repositories;
using Task2.Cmd.Interfaces;
using Task2.Cmd.Models;

namespace Task2.Cmd.DataManager
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Context _context;
        private VisitorRepository _visitors;
        private HotelRoomRepository _hotelRooms;
        private InternalHotelInformationRepository _internalHotelInformation;
        public UnitOfWork() => _context = new Context();
        public IRepository<Visitor> VisitorRepository
        {
            get
            {
                if (_visitors == null)
                    _visitors = new VisitorRepository(_context);
                return _visitors;
            }
        }
        public IRepository<HotelRoom> HotelRoomRepository 
        {
            get
            {
                if (_hotelRooms == null)
                    _hotelRooms = new HotelRoomRepository(_context);
                return _hotelRooms;
            }
        }
        public IRepository<InternalHotelInformation> InternalHotelInformation
        {
            get
            {
                if (_internalHotelInformation == null)
                    _internalHotelInformation = new InternalHotelInformationRepository(_context);
                return _internalHotelInformation;
            }
        }
        public void Save() => _context.Save();
    }
}
