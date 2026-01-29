using SchoolProject.Core.Features.Students.Commands.Models;
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
        public void EditStudentCommandMapping()
        {
            // Mapping configuration between EditStudentCommand (Source) and Student (Destination)
            CreateMap<EditStudentCommand, Student>()
                // Map DepartmentId explicitly because the names differ (Source: DepartmentId -> Destination: DeptId)
                // Map Id explicitly because the names differ (Source: Id -> Destination: StudId)
                // All other properties (Name, Address, etc.) are mapped automatically because the names match.
                .ForMember(dest => dest.StudId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DeptId, opt => opt.MapFrom(src => src.DepartmentId));
        }
    }
}
