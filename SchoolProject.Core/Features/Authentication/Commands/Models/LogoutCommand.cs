using MediatR;
using SchoolProject.Core.Bases;

namespace SchoolProject.Core.Features.Authentication.Commands.Models
{
    public class LogoutCommand : IRequest<Response<string>>
    {
        public string AccessToken { get; set; }
    }
}