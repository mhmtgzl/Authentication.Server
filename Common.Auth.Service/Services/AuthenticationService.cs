using Common.Auth.Core.Configuration;
using Common.Auth.Core.DTO;
using Common.Auth.Core.Models;
using Common.Auth.Core.Repositories;
using Common.Auth.Core.Services;
using Common.Auth.Core.UnitOfWork;
using Common.Shared.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Auth.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {

        private readonly List<Client> _clients;
        private readonly ITokenService _tokenService;
        private readonly UserManager<UserApp> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<UserRefreshToken> _refreshTokenRepository;

        public AuthenticationService(
            IOptions<List<Client>> options,
            ITokenService tokenService,
            UserManager<UserApp> userManager,
            IUnitOfWork unitOfWork,
            IRepository<UserRefreshToken> refreshTokenRepository)
        {
            _clients = options.Value;
            _tokenService = tokenService;
            _userManager = userManager;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;

        }

        public async Task<Response<TokenDto>> CreateTokenAsync(LoginDto login)
        {
            if (login == null)
                throw new ArgumentNullException(nameof(login));

            var user = await _userManager.FindByEmailAsync(login.Email);

            if (user == null)
                return Response<TokenDto>.Fail("Email or Password is wrong", 400, true);

            if (!await _userManager.CheckPasswordAsync(user, login.Password))
            {
                return Response<TokenDto>.Fail("Email or Password is wrong", 400, true);
            }

            var token = _tokenService.CreateToken(user);

            var userRefreshToken = await _refreshTokenRepository.Where(x => x.UserId == user.Id).SingleOrDefaultAsync();

            if (userRefreshToken == null)
                await _refreshTokenRepository.AddAsync(new UserRefreshToken { UserId = user.Id, Code = token.RefreshToken, Expiration = token.RefreshTokenExpiration });
            else
            {
                userRefreshToken.Code = token.RefreshToken;
                userRefreshToken.Expiration = token.RefreshTokenExpiration;
            }

            await _unitOfWork.CommitAsync();

            return Response<TokenDto>.Success(token, 200);
        }

        public Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLogin)
        {
            var client = _clients.SingleOrDefault(x => x.Id == clientLogin.ClientId && x.Secret == clientLogin.ClientSecret);

            if (client == null)
                return Response<ClientTokenDto>.Fail("ClientId or ClientSecret not found", 404, true);

            var token = _tokenService.CreateTokenByClient(client);

            return Response<ClientTokenDto>.Success(token, 200);

        }

        public async Task<Response<TokenDto>> CreateTokenByRefreshTokenAsync(string refreshToken)
        {
            var token = await _refreshTokenRepository.Where(x => x.Code.Equals(refreshToken)).SingleOrDefaultAsync();

            if (token == null)
                return Response<TokenDto>.Fail("Refresh token not found", 404, true);

            var user = await _userManager.FindByIdAsync(token.UserId);

            if (user == null)
                return Response<TokenDto>.Fail("User not found", 404, true);

            var newToken = _tokenService.CreateToken(user);

            token.Code = newToken.RefreshToken;
            token.Expiration = newToken.RefreshTokenExpiration;

            await _unitOfWork.CommitAsync();

            return Response<TokenDto>.Success(newToken, 200);

        }

        public async Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken)
        {
            var refresh = await _refreshTokenRepository.Where(x => x.Code.Equals(refreshToken)).SingleOrDefaultAsync();

            if (refresh == null)
                return Response<NoDataDto>.Fail("Refreshtoken not found", 404, true);

            _refreshTokenRepository.Remove(refresh);

            await _unitOfWork.CommitAsync();

            return Response<NoDataDto>.Success(200);


        }
    }
}
