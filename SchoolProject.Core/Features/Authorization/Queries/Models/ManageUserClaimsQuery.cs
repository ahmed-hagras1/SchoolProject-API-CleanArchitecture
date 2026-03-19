using MediatR;
using SchoolProject.Core.Bases;
using SchoolProject.Data.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Authorization.Queries.Models
{
    public class ManageUserClaimsQuery : IRequest<Response<ManageUserClaimsResult>>
    {
        public int UserId { get; set; }
        public ManageUserClaimsQuery(int userId)
        {
            UserId = userId;
        }
    }
}
