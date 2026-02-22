using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.API.Base;
using SchoolProject.Core.Features.ApplicationUser.Commands.Models;
using SchoolProject.Core.Features.ApplicationUser.Queries.Models;
using SchoolProject.Core.Features.ApplicationUser.Queries.Results;
using SchoolProject.Core.Features.Students.Commands.Models;
using SchoolProject.Core.Wrappers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
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
        [HttpGet(Router.ApplicationUserRouting.GetPaginatedList)]
        public async Task<IActionResult> GetApplicationUserPaginatedListAsync([FromQuery] GetUserPaginatedListQuery getApplicationUserPaginatedListQuery)
        {
            return Ok(await Mediator.Send(getApplicationUserPaginatedListQuery));
        }
        [HttpGet(Router.ApplicationUserRouting.GetUserById)]
        public async Task<IActionResult> GetApplicationUserByIdAsync([FromRoute] int id)
        {
            return NewResult(await Mediator.Send(new GetUserByIdQuery(id)));
            //var response = await Mediator.Send(getApplicationUserByIdQuery);
            //return Ok(response);

        }
        [HttpPut(Router.ApplicationUserRouting.Update)]
        public async Task<IActionResult> UpdateApplicationUser([FromBody] UpdateApplicationUserCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }
        [HttpDelete(Router.ApplicationUserRouting.Delete)]
        public async Task<IActionResult> DeleteApplicationUser([FromRoute] int id)
        {
            return NewResult(await Mediator.Send(new DeleteApplicationUserCommand(id)));
        }
        [HttpPut(Router.ApplicationUserRouting.ChangePassword)]
        public async Task<IActionResult> ChangeUserPassword([FromBody] ChangeUserPasswordCommand command)
        {
            return NewResult(await Mediator.Send(command));

        }
    }
}
