using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.API.Base;
using SchoolProject.Core.Features.Instructors.Commands.Models;

namespace SchoolProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorController : AppControllerBase
    {
        // 1. Only Admins should be allowed to change salaries!
        [Authorize(Roles = "Admin")]

        // 2. We use HttpPut because we are updating an existing record
        [HttpPut("UpdateSalary")]
        public async Task<IActionResult> UpdateSalary([FromBody] UpdateInstructorSalaryCommand command)
        {
            // Send the command to MediatR
            var response = await Mediator.Send(command);

            // Return the formatted response
            return NewResult(response);
        }
    }
}
