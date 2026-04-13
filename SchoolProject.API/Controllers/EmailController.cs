using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.API.Base;
using SchoolProject.Core.Features.Email.Commands.Models;
using SchoolProject.Core.Features.Students.Commands.Models;
using Router = SchoolProject.Core.AppMetaData.Router;

namespace SchoolProject.API.Controllers
{
    [ApiController]
    public class EmailController : AppControllerBase
    {
        [HttpPost(Router.EmailRouting.SendEmail)]
        public async Task<IActionResult> SendEmailAsync([FromBody] SendEmailCommand sendEmailCommand)
        {
            return NewResult(await Mediator.Send(sendEmailCommand));
        }
    }
}
