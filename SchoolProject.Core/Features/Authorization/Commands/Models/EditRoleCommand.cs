using MediatR;
using SchoolProject.Core.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Authorization.Commands.Models
{
    public class EditRoleCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
    }
}
