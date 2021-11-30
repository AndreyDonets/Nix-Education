using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Task5.Infrastructure.Security
{
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer";
        public const string AUDIENCE = "MyAuthClient";
        const string KEY = "mysupersecret_secretkey!123"; 
        public const int LIFETIME = 15;
        public static SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }
}
