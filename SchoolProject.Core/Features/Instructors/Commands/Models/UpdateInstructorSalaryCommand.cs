using MediatR;
using SchoolProject.Core.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Instructors.Commands.Models
{
    public class UpdateInstructorSalaryCommand : IRequest<Response<string>>
    {
        public int InstructorId { get; set; }
        public decimal NewSalary { get; set; }
    }
}
