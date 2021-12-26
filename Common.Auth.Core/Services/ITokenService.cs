using Common.Auth.Core.Configuration;
using Common.Auth.Core.DTO;
using Common.Auth.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Auth.Core.Services
{
    public interface ITokenService
    {
        TokenDto CreateToken(UserApp user);
        ClientTokenDto CreateTokenByClient(Client client);

    }
}
