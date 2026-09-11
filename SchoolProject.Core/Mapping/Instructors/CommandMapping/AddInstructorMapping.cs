using SchoolProject.Core.Features.Departments.Queries.Results;
using SchoolProject.Core.Features.Instructors.Commands.Models;
using SchoolProject.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Mapping.Instructors
{
    public partial class InstructorProfile
    {
        public void AddInstructorMapping()
        {
            CreateMap<AddInstructorCommand, Instructor>()
                // Ignore the Image property so AutoMapper doesn't crash trying to map IFormFile to string
                .ForMember(dest => dest.Image, opt => opt.Ignore());

                // AutoMapper will automatically map Name, Address, Position, Salary, DeptId, and SupervisorId 
                // because the property names match exactly!
        }
    }
}
