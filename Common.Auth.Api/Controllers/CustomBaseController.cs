using Common.Shared.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Common.Auth.Api.Controllers
{
   
    public class CustomBaseController : ControllerBase
    {
        public IActionResult ActionResultIntance<T>(Response<T> response) where T:class
        {
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };

        }
    }
}
