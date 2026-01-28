using MediatR;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Core.AppMetaData;
using SchoolProject.Core.Features.Students.Queries.Models;
using Router = SchoolProject.Core.AppMetaData.Router;

namespace SchoolProject.API.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        #region Fields
        private readonly IMediator _mediator;
        #endregion
        #region Constructor
        public StudentController(IMediator mediator) 
        {
            _mediator = mediator;
        }
        #endregion
        #region Endpoints
        //[HttpGet("GetStudentsList")]
        [HttpGet(Router.StudentRouting.List)]
        public async Task<IActionResult> GetStudentsListAsync()
        {
            var students = await _mediator.Send(new GetStudentListQuery());
            return Ok(students);
        }
        //[HttpGet("GetStudentById/{id}")]
        [HttpGet(Router.StudentRouting.GetById)]
        public async Task<IActionResult> GetStudentById([FromRoute]int id)
        {
            var student = await _mediator.Send(new GetStudentByIdQuery(id));
            return Ok(student);
        }
        #endregion
    }
}
