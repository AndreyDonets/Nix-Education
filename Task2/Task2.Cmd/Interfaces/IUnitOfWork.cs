using System.Threading.Tasks;
using Task2.Cmd.Models;

namespace Task2.Cmd.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Visitor> VisitorRepository { get; }
        IRepository<HotelRoom> HotelRoomRepository { get; }
        IRepository<InternalHotelInformation> InternalHotelInformation { get; }

        void Save();
    }
}
