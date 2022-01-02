using Common.Auth.Core.DTO;
using Common.Auth.Core.Services;
using Common.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Common.Auth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : CustomBaseController
    {

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto userDto)
        {
            
            return ActionResultIntance(await _userService.CreateUserAsync(userDto));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var userName = HttpContext.User.Identity?.Name;
            return ActionResultIntance(await _userService.GetUserByNameAsync(userName));
        }
    }
}
