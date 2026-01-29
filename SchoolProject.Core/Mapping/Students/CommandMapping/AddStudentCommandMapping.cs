using SchoolProject.Core.Features.Students.Commands.Models;
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
        public void AddStudentCommandMapping()
        {
            // Mapping configuration between AddStudentCommand (Source) and Student (Destination)
            CreateMap<AddStudentCommand, Student>()
                // Map DepartmentId explicitly because the names differ (Source: DepartmentId -> Destination: DeptId)
                // All other properties (Name, Address, etc.) are mapped automatically because the names match.
                .ForMember(dest => dest.DeptId, opt => opt.MapFrom(src => src.DepartmentId));
        }
    }
}
