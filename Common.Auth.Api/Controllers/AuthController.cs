using Common.Auth.Core.DTO;
using Common.Auth.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Common.Auth.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : CustomBaseController
    {

        private readonly IAuthenticationService authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken(LoginDto loginDto)
        {
            var result = await authenticationService.CreateTokenAsync(loginDto);

            return ActionResultIntance(result);

        }

        [HttpPost]
        public IActionResult CreateTokenByClient(ClientLoginDto loginDto)
        {
            var result = authenticationService.CreateTokenByClient(loginDto);

            return ActionResultIntance(result);

        }

        [HttpPost]
        public async Task<IActionResult> RevokeRefreshToken(RefreshTokenDTo refreshToken)
        {
            var result = await authenticationService.RevokeRefreshToken(refreshToken.Token);

            return ActionResultIntance(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTokenByRefreshToken(RefreshTokenDTo refreshToken)
        {
            var result = await authenticationService.CreateTokenByRefreshTokenAsync(refreshToken.Token);

            return ActionResultIntance(result);

        }
    }
}
