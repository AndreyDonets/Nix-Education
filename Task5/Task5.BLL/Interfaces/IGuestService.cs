using System.Threading.Tasks;
using Task5.BLL.DTO;

namespace Task5.BLL.Interfaces
{
    public interface IGuestService : IBaseService<GuestDTO>
    {
        GuestDTO GetGuestByPassport(string passport);
        Task<GuestDTO> GetGuestByPassportAsync(string passport);
    }
}