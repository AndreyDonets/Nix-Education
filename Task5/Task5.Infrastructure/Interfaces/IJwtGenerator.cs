using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Task5.Infrastructure.Interfaces
{
    public interface IJwtGenerator
    {
        Task<string> CreateToken(IdentityUser user);
    }
}