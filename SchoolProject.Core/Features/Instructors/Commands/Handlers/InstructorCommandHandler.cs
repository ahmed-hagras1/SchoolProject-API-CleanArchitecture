using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Instructors.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Data.Entities;
using SchoolProject.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Instructors.Commands.Handlers
{
    public class InstructorCommandHandler : ResponseHandler,
        IRequestHandler<UpdateInstructorSalaryCommand, Response<string>>,
        IRequestHandler<AddInstructorCommand, Response<string>>
    {
        private readonly IInstructorService _instructorService;
        private readonly IStringLocalizer _stringLocalizer;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public InstructorCommandHandler(IInstructorService instructorService,
                                        IStringLocalizer<SharedResources> stringLocalizer,
                                        IMapper mapper,
                                        IFileService fileService) : base(stringLocalizer)
        {
            _instructorService = instructorService;
            _stringLocalizer = stringLocalizer;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<Response<string>> Handle(UpdateInstructorSalaryCommand request, CancellationToken cancellationToken)
        {
            var result = await _instructorService.UpdateSalaryProcedureAsync(request.InstructorId, request.NewSalary);

            if (result == "Success")
                return Success<string>("Instructor salary updated successfully using Stored Procedure.");

            return BadRequest<string>("Failed to update salary. Instructor may not exist.");
        }

        public async Task<Response<string>> Handle(AddInstructorCommand request, CancellationToken cancellationToken)
        {
            // 2. Map the basic text properties (AutoMapper will ignore the IFormFile)
            var instructorMapper = _mapper.Map<Instructor>(request);

            // 3. Handle the Image Upload
            if (request.Image != null)
            {
                // Call the function you just built!
                var imageUrl = await _fileService.UploadImage("Images/Instructors", request.Image);

                // Assign the returned path (e.g., "/Images/Instructors/12345.jpg") to the database entity
                instructorMapper.Image = imageUrl;
            }

            // 4. Save the new Instructor to the database
            var result = await _instructorService.AddInstructorAsync(instructorMapper);

            // 5. Return success or failure
            if (result == "Success")
            {
                return Created<string>("Instructor Added Successfully");
            }

            return BadRequest<string>("Failed to add the instructor");
        }
    }
}
