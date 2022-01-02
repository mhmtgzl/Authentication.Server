using Common.Auth.Core.DTO;
using Common.Auth.Core.Models;
using Common.Auth.Core.Services;
using Common.Shared.DTO;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Auth.Service.Services
{
    public class UserService : IUserService
    {

        private readonly UserManager<UserApp> _userManager;

        public UserService(UserManager<UserApp> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto userDto)
        {
            var user = new UserApp { 
                Email = userDto.Email,
                UserName = userDto.UserName,
            };

            var result = await _userManager.CreateAsync(user,userDto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x=>x.Description).ToList();

                return Response<UserAppDto>.Fail(new ErrorDto(errors, true), 400);

            }

            return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);

        }

        public async Task<Response<UserAppDto>> GetUserByNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if(user == null)
            return Response<UserAppDto>.Fail("User not found",404,true);

            return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);

        }
    }
}
