using Common.Auth.Core.Configuration;
using Common.Auth.Core.DTO;
using Common.Auth.Core.Models;
using Common.Auth.Core.Services;
using Common.Shared.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common.Auth.Service.Services
{
    public class TokenService : ITokenService
    {

        private readonly UserManager<UserApp> userManager;
        private readonly CustomTokenOptions customTokenOptions;

        public TokenService(UserManager<UserApp> userManager,
             IOptions<CustomTokenOptions> options)
        {
            this.userManager = userManager;
            this.customTokenOptions = options.Value;
            List<int> s = new List<int>();
            string a;
            a.Length()
        }


        private string CreateRefreshToken()
        {

            var numberByte=new byte[32];
            using var rnd = RandomNumberGenerator.Create();

            rnd.GetBytes(numberByte);

            return Convert.ToBase64String(numberByte);

        }

        private IEnumerable<Claim> GetClaim(UserApp userApp,List<string> audiences)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,userApp.Id),
                new Claim(JwtRegisteredClaimNames.Email,userApp.Email),
                new Claim(ClaimTypes.Name,userApp.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString("D"))
            };

            claims.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

            return claims;
        }

        public TokenDto CreateToken(UserApp user)
        {
            throw new NotImplementedException();
        }

        public ClientTokenDto CreateTokenByClient(Client client)
        {
            throw new NotImplementedException();
        }
    }
}
