using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using SchoolProject.Core.Features.Students.Queries.Models;
using SchoolProject.API.Base;
using SchoolProject.Core.Features.Departments.Queries.Models;
using Router = SchoolProject.Core.AppMetaData.Router;

namespace SchoolProject.API.Controllers
{
    [ApiController]
    public class DepartmentController : AppControllerBase
    {
        [HttpGet(Router.DepartmentRouting.List)]
        public async Task<IActionResult> GetDepartmentsListAsync()
        {
            return NewResult (await Mediator.Send(new GetDepartmentListQuery()));
        }
        [HttpGet(Router.DepartmentRouting.GetById)]
        public async Task<IActionResult> GetDepartmentByIdAsync([FromQuery] GetDepartmentByIdQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }
    }
}
