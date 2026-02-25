using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.API.Base;
using SchoolProject.Core.AppMetaData;
using SchoolProject.Core.Features.Students.Commands.Models;
using SchoolProject.Core.Features.Students.Queries.Models;
using Router = SchoolProject.Core.AppMetaData.Router;

namespace SchoolProject.API.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    [Authorize] // This means all endpoints in this controller require authentication. You can override this with [AllowAnonymous] on specific actions if needed.
    public class StudentController : AppControllerBase
    {
       
        #region Endpoints
        //[HttpGet("GetStudentsList")]
        [HttpGet(Router.StudentRouting.List)]
        public async Task<IActionResult> GetStudentsListAsync()
        {
            return NewResult(await Mediator.Send(new GetStudentListQuery()));
        }
        [HttpGet(Router.StudentRouting.Paginated)]
        [AllowAnonymous] // This endpoint can be accessed without authentication, overriding the [Authorize] attribute at the controller level.
        public async Task<IActionResult> GetStudentsPaginatedAsync([FromQuery] GetStudentPaginatedListQuery query)
        {
            var response =  await Mediator.Send(query);
            return Ok(response);
        }
        //[HttpGet("GetStudentById/{id}")]
        [HttpGet(Router.StudentRouting.GetById)]
        public async Task<IActionResult> GetStudentById([FromRoute] int id)
        {
            return NewResult(await Mediator.Send(new GetStudentByIdQuery(id)));
        }
        [HttpPost(Router.StudentRouting.Add)]
        public async Task<IActionResult> AddStudentAsync([FromBody] AddStudentCommand addStudentCommand)
        {
            return NewResult(await Mediator.Send(addStudentCommand));
        }
        [HttpPut(Router.StudentRouting.Update)]
        public async Task<IActionResult> UpdateStudentAsync([FromBody] EditStudentCommand updateStudentCommand)
        {
            return NewResult(await Mediator.Send(updateStudentCommand));
        }
        [HttpDelete(Router.StudentRouting.Delete)]
        public async Task<IActionResult> DeleteStudentByIdAsync([FromRoute] int id)
        {
            return NewResult(await Mediator.Send(new DeleteStudentByIdCommand(id)));
        }

        #endregion
    }
}
