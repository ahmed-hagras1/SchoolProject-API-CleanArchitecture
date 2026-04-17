using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Instructors.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Instructors.Commands.Handlers
{
    public class UpdateInstructorSalaryCommandHandler : ResponseHandler, IRequestHandler<UpdateInstructorSalaryCommand, Response<string>>
    {
        private readonly IInstructorService _instructorService;

        public UpdateInstructorSalaryCommandHandler(IInstructorService instructorService, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _instructorService = instructorService;
        }

        public async Task<Response<string>> Handle(UpdateInstructorSalaryCommand request, CancellationToken cancellationToken)
        {
            var result = await _instructorService.UpdateSalaryProcedureAsync(request.InstructorId, request.NewSalary);

            if (result == "Success")
                return Success<string>("Instructor salary updated successfully using Stored Procedure.");

            return BadRequest<string>("Failed to update salary. Instructor may not exist.");
        }
    }
}
