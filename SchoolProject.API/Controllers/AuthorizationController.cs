using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.API.Base;
using SchoolProject.Core.Features.Authorization.Commands.Models;
using Router = SchoolProject.Core.AppMetaData.Router;

namespace SchoolProject.API.Controllers
{
    [ApiController]
    // This controller is protected by the Authorize attribute, allowing only users with "Admin" or "User" roles to access its endpoints.
    [Authorize(Roles = "Admin,User")]
    // This controller is protected by the Authorize attribute, allowing only users with "Admin", and "User" roles to access its endpoints.
    //[Authorize(Roles = "Admin")]
    //[Authorize(Roles = "User")]

    public class AuthorizationController : AppControllerBase
    {
        [HttpPost(Router.AuthorizationRouting.AddNewRole)]
        // This endpoint is further restricted to only allow users with the "Admin" role to access it, ensuring that only administrators can add new roles.
        // [Authorize(Roles = "Admin")]
        // Bind directly to the command
        public async Task<IActionResult> AddNewRole([FromForm] AddRoleCommand command)
        {
            var response = await Mediator.Send(command);

            // Let your base controller handle the HTTP status codes automatically!
            return NewResult(response);
        }
        [HttpPut(Router.AuthorizationRouting.EditRole)]
        public async Task<IActionResult> EditRole([FromForm] EditRoleCommand command)
        {
            var response = await Mediator.Send(command);

            // Let your base controller handle the HTTP status codes automatically!
            return NewResult(response);
        }
        [HttpDelete(Router.AuthorizationRouting.DeleteRole)]
        public async Task<IActionResult> DeleteRole([FromRoute] int id)
        {
            var response = await Mediator.Send(new DeleteRoleCommand(id));

            // Let your base controller handle the HTTP status codes automatically!
            return NewResult(response);
        }
    }
}
