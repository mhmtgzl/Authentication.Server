using Common.Auth.Core.Configuration;
using Common.Auth.Core.DTO;
using Common.Auth.Core.Models;
using Common.Auth.Core.Services;
using Common.Shared.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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

        private IEnumerable<Claim> GetClaimsByClient(Client client)
        {
            var claims = new List<Claim>();
            claims.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("D"));
            new Claim(JwtRegisteredClaimNames.Sub, client.Id.ToString());
            return claims;

        }

        public TokenDto CreateToken(UserApp user)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(customTokenOptions.AccessTokenExpiration);
            var refreshTokenExpiration = DateTime.Now.AddMinutes(customTokenOptions.RefreshTokenExpiration);

            var security = SignService.GetSymmetricSecurityKey(customTokenOptions.SecurityKey);
            SigningCredentials signingCredentials = new SigningCredentials(security, SecurityAlgorithms.HmacSha256Signature);
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: customTokenOptions.Issuer,
                notBefore: DateTime.Now,
                expires: accessTokenExpiration,
                signingCredentials: signingCredentials,
                claims: GetClaim(user, customTokenOptions.Audience));

            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(jwtSecurityToken);

            return new TokenDto
            {
                AccessToken = token,
                AccessTokenExpiration = accessTokenExpiration,
                RefreshToken=CreateRefreshToken(),
                RefreshTokenExpiration=refreshTokenExpiration
            };

        }

        public ClientTokenDto CreateTokenByClient(Client client)
        {
            throw new NotImplementedException();
        }
    }
}
