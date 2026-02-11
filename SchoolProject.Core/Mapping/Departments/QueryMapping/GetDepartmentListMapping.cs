using SchoolProject.Core.Features.Departments.Queries.Results;
using SchoolProject.Core.Features.Students.Queries.Results;
using SchoolProject.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Mapping.Departments
{
    public partial class DepartmentProfile
    {
        public void GetDepartmentListMapping()
        {
            CreateMap<Department, GetDepartmentListResponse>()
                .ForMember(destination => destination.DepartmentName, opt => opt.MapFrom(src => src.DeptName))
                .ForMember(destination => destination.ManagerName,
                opt => opt.MapFrom(src => src.InstructorManager != null? src.InstructorManager.Name : string.Empty));
        }
    }
}