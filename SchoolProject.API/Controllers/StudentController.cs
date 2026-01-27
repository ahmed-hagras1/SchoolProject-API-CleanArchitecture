using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Core.Features.Students.Queries.Models;

namespace SchoolProject.API.Controllers
{
    [Route("api/[controller]")]
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
        [HttpGet("GetStudentsList")]
        public async Task<IActionResult> GetStudentsListAsync()
        {
            var students = await _mediator.Send(new GetStudentListQuery());
            return Ok(students);
        }
        #endregion
    }
}
