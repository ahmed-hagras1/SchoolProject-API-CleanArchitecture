using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.API.Base;
using SchoolProject.Core.Features.Authentication.Commands.Models;
using Route = SchoolProject.Core.AppMetaData.Router;

namespace SchoolProject.API.Controllers
{
    [ApiController]
    public class AuthenticationController : AppControllerBase
    {
        [HttpPost(Route.AuthenticationRouting.SignIn)]
        public async Task<IActionResult> Create([FromForm]SignInCommand signInCommand)
        {
            var response = await Mediator.Send(signInCommand);

            if (response.Succeeded)
            {
                return NewResult(response);
            }

            return BadRequest(response);
        }
        [HttpPost(Route.AuthenticationRouting.RefreshToken)]
        public async Task<IActionResult> RefreshToken([FromForm] RefreshTokenCommand refreshTokenCommand)
        {
            var response = await Mediator.Send(refreshTokenCommand);

            if (response.Succeeded)
            {
                return NewResult(response);
            }

            return BadRequest(response);
        }
    }
}
