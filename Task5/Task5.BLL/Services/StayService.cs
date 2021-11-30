using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task5.BLL.DTO;
using Task5.BLL.Interfaces;
using Task5.DAL.Entities;
using Task5.DAL.Interfaces;

namespace Task5.BLL.Services
{
    public class StayService : IStayService
    {
        private IUnitOfWork db;
        private IRoomService roomService;
        private IGuestRepository guestRepository;

        public StayService(IUnitOfWork db, IRoomService roomService, IGuestRepository guestRepository)
        {
            this.db = db;
            this.roomService = roomService;
            this.guestRepository = guestRepository;
        }

        private IMapper GetMapperToStayDTO() => new MapperConfiguration(cfg => cfg.CreateMap<Stay, StayDTO>()).CreateMapper();
        private IMapper GetMapperToStay() => new MapperConfiguration(cfg => cfg.CreateMap<StayDTO, Stay>()).CreateMapper();

        public IEnumerable<StayDTO> GetAll() => GetMapperToStayDTO().Map<IEnumerable<Stay>, List<StayDTO>>(db.StayRepository.GetAll());
        public StayDTO Get(Guid id) => GetMapperToStayDTO().Map<Stay, StayDTO>(db.StayRepository.Get(id));
        public IEnumerable<StayDTO> GetStaysByRoomId(Guid roomId)
        {
            var stays = db.StayRepository.GetAll();
            return GetMapperToStayDTO().Map<IEnumerable<Stay>, List<StayDTO>>(stays.Where(x => x.RoomId == roomId));
        }
        public IEnumerable<StayDTO> GetStaysByRoomNumber(int roomNumber)
        {
            var room = roomService.GetRoomByNumber(roomNumber);
            if (room == null)
                throw new ArgumentNullException();
            var stays = db.StayRepository.GetAll();
            return GetMapperToStayDTO().Map<IEnumerable<Stay>, List<StayDTO>>(stays.Where(x => x.RoomId == room.Id));
        }
        public IEnumerable<StayDTO> GetStaysByGuestId(Guid guestId)
        {
            var stays = db.StayRepository.GetAll();
            return GetMapperToStayDTO().Map<IEnumerable<Stay>, List<StayDTO>>(stays.Where(x => x.GuestId == guestId));
        }
        public IEnumerable<StayDTO> GetStaysByGuestPassport(string passport)
        {
            var guest = guestRepository.GetByPassport(passport);
            if (guest == null)
                throw new ArgumentNullException();
            var stays = db.StayRepository.GetAll();
            return GetMapperToStayDTO().Map<IEnumerable<Stay>, List<StayDTO>>(stays.Where(x => x.RoomId == guest.Id));
        }
        public void Create(StayDTO item) => db.StayRepository.Create(GetMapperToStay().Map<StayDTO, Stay>(item));
        public void Update(StayDTO item)
        {
            var stay = db.StayRepository.Get(item.Id);
            stay.GuestId = item.GuestId;
            stay.CheckedIn = item.CheckedIn;
            stay.CheckedOut = item.CheckedOut;
            stay.EndDate = item.EndDate;
            stay.StartDate = item.StartDate;
            stay.RoomId = item.RoomId;
            db.StayRepository.Update(stay);
        }

        public void Delete(Guid id) => db.StayRepository.Delete(id);
        public void Save() => db.Save();
        public async Task<IEnumerable<StayDTO>> GetAllAsync() => GetMapperToStayDTO().Map<IEnumerable<Stay>, List<StayDTO>>(await db.StayRepository.GetAllAsync());
        public async Task<StayDTO> GetAsync(Guid id) => GetMapperToStayDTO().Map<Stay, StayDTO>(await db.StayRepository.GetAsync(id));
        public async Task<IEnumerable<StayDTO>> GetStaysByRoomIdAsync(Guid roomId)
        {
            var stays = await db.StayRepository.GetAllAsync();
            return GetMapperToStayDTO().Map<IEnumerable<Stay>, List<StayDTO>>(stays.Where(x => x.RoomId == roomId));
        }
        public async Task<IEnumerable<StayDTO>> GetStaysByRoomNumberAsync(int roomNumber)
        {
            var room = await roomService.GetRoomByNumberAsync(roomNumber);
            if (room == null)
                throw new ArgumentNullException();
            var stays = await db.StayRepository.GetAllAsync();
            return GetMapperToStayDTO().Map<IEnumerable<Stay>, List<StayDTO>>(stays.Where(x => x.RoomId == room.Id));
        }
        public async Task<IEnumerable<StayDTO>> GetStaysByGuestIdAsync(Guid guestId)
        {
            var stays = await db.StayRepository.GetAllAsync();
            return GetMapperToStayDTO().Map<IEnumerable<Stay>, List<StayDTO>>(stays.Where(x => x.GuestId == guestId));
        }
        public async Task<IEnumerable<StayDTO>> GetStaysByGuestPassportAsync(string passport)
        {
            var guest = await guestRepository.GetByPassportAsync(passport);
            if (guest == null)
                throw new ArgumentNullException();
            var stays = await db.StayRepository.GetAllAsync();
            return GetMapperToStayDTO().Map<IEnumerable<Stay>, List<StayDTO>>(stays.Where(x => x.RoomId == guest.Id));
        }
        public async Task CreateAsync(StayDTO item) => await db.StayRepository.CreateAsync(GetMapperToStay().Map<StayDTO, Stay>(item));
        public async Task UpdateAsync(StayDTO item)
        {
            var stay = db.StayRepository.Get(item.Id);
            stay.GuestId = item.GuestId;
            stay.CheckedIn = item.CheckedIn;
            stay.CheckedOut = item.CheckedOut;
            stay.EndDate = item.EndDate;
            stay.StartDate = item.StartDate;
            stay.RoomId = item.RoomId;
            await db.StayRepository.UpdateAsync(stay);
        }

        public async Task DeleteAsync(Guid id) => await db.StayRepository.DeleteAsync(id);
        public void Dispose() => db.Dispose();
    }
}
