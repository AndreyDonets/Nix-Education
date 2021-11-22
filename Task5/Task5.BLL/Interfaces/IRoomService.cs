using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task5.BLL.DTO;

namespace Task5.BLL.Interfaces
{
    public interface IRoomService : IBaseService<RoomDTO>
    {
        RoomDTO GetRoomByNumber(int number);
        Task<RoomDTO> GetRoomByNumberAsync(int number);
        IEnumerable<RoomDTO> GetRoomsByCategoryId(Guid categoryId);
        Task<IEnumerable<RoomDTO>> GetRoomsByCategoryIdAsync(Guid categoryId);
        IEnumerable<RoomDTO> GetRoomsByCategoryName(string categoryName);
        Task<IEnumerable<RoomDTO>> GetRoomsByCategoryNameAsync(string categoryName);
    }
}
