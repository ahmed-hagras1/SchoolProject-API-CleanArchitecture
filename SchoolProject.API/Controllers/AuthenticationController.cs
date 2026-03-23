using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.API.Base;
using SchoolProject.Core.Features.Authentication.Commands.Models;
using SchoolProject.Core.Features.Authentication.Queries.Models;
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
        [HttpPost(Route.AuthenticationRouting.Logout)]
        [Authorize] // 1. This ensures only logged-in users can access this endpoint
        public async Task<IActionResult> Logout()
        {
            // 2. Automatically extract the token from the "Authorization: Bearer <token>" header
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            // Fallback: If GetTokenAsync doesn't find it, extract it manually from the header
            if (string.IsNullOrEmpty(accessToken))
            {
                var authHeader = Request.Headers["Authorization"].FirstOrDefault();
                accessToken = authHeader?.Replace("Bearer ", "").Trim();
            }

            // 3. Send it to your MediatR Command Handler
            var command = new LogoutCommand
            {
                AccessToken = accessToken
            };

            var response = await Mediator.Send(command);

            // 4. Return the standardized response based on the status code
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(response);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return Unauthorized(response);
            }

            return BadRequest(response);
        }
        [HttpGet(Route.AuthenticationRouting.ConfirmEmail)]
        // لا تضع [Authorize] هنا، لأن المستخدم لم يقم بتسجيل الدخول بعد!
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailQuery query)
        {
            var response = await Mediator.Send(query);
            return NewResult(response);
        }
    }
}
