using MediatR;
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
        [HttpPut(Router.StudentRouting.Edit)]
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
