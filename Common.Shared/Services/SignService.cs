using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Common.Shared.Services
{
    public static class SignService
    {
        private static string securityKey = "huhaıuhoasdho2093*ajk23-ü-20*h324hkhsdhk3h8kjsl";

        public static SecurityKey GetSymmetricSecurityKey()
        {
            return GetSymmetricSecurityKey(securityKey);
        }

        public static SecurityKey GetSymmetricSecurityKey(string securityKey)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
        }
    }
}
