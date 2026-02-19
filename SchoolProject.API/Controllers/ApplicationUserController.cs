using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.API.Base;
using SchoolProject.Core.Features.ApplicationUser.Commands.Models;
using SchoolProject.Core.Features.Students.Commands.Models;
using Router = SchoolProject.Core.AppMetaData.Router;

namespace SchoolProject.API.Controllers
{
    [ApiController]
    public class ApplicationUserController : AppControllerBase
    {
        [HttpPost(Router.ApplicationUserRouting.Add)]
        public async Task<IActionResult> AddApplicationUserAsync([FromBody] AddApplicationUserCommand addApplicationUserCommand)
        {
            return NewResult(await Mediator.Send(addApplicationUserCommand));
        }
    }
}
