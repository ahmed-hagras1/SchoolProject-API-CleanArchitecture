using MediatR;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.ApplicationUser.Queries.Results;
using SchoolProject.Core.Wrappers;
using SchoolProject.Data.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Views.Queries.Models
{
    // This query asks for a Response containing a List of the View Entity
    public class GetDepartmentStudentCountListQuery : IRequest<Response<List<ViewDepartmentStudentCount>>>
    {
        // No properties needed here because we are just getting ALL data without any filters (like Id)
    }
}
