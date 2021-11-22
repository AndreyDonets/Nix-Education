using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task5.BLL.DTO;

namespace Task5.BLL.Interfaces
{
    public interface IStayService : IBaseService<StayDTO>
    {
        IEnumerable<StayDTO> GetStaysByRoomId(Guid roomId);
        Task<IEnumerable<StayDTO>> GetStaysByRoomIdAsync(Guid roomId);
        IEnumerable<StayDTO> GetStaysByRoomNumber(int roomNumber);
        Task<IEnumerable<StayDTO>> GetStaysByRoomNumberAsync(int roomNumber);
        IEnumerable<StayDTO> GetStaysByGuestId(Guid guestId);
        Task<IEnumerable<StayDTO>> GetStaysByGuestIdAsync(Guid guestId);
        IEnumerable<StayDTO> GetStaysByGuestPassport(string passport);
        Task<IEnumerable<StayDTO>> GetStaysByGuestPassportAsync(string passport);
    }
}
