using SchoolProject.Core.Features.Students.Queries.Results;
using SchoolProject.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Mapping.Students
{
    public partial class StudentProfile
    {
        public void GetStudentByIdMapping()
        {
            // Mapping configuration between Student entity and GetStudentListResponse DTO.
            // Maps DepartmentName from the related Department entity.
            // Maps only DepartmentName because other properties have the same names in both classes Student and GetStudentListResponse.
            CreateMap<Student, GetStudentResponse>()
                .ForMember(destination => destination.DepartmentName, opt => opt.MapFrom(src => src.Department.DeptName));
        }
    }
}
