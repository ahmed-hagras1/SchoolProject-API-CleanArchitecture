using AutoMapper;
using MediatR;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Students.Commands.Models;
using SchoolProject.Core.Features.Students.Queries.Models;
using SchoolProject.Core.Features.Students.Queries.Results;
using SchoolProject.Data.Entities;
using SchoolProject.Service.Abstracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Students.Commands.Handlers
{
    public class StudentCommandHandler : ResponseHandler,
        IRequestHandler<AddStudentCommand, Response<string>>,
        IRequestHandler<EditStudentCommand, Response<string>>,
        IRequestHandler<DeleteStudentByIdCommand, Response<string>>
    {
        #region Fields
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public StudentCommandHandler(IStudentService studentService, IMapper mapper)
        {
            _studentService = studentService;
            _mapper = mapper;
        }
        #endregion

        #region Handle Method
        public async Task<Response<string>> Handle(AddStudentCommand request, CancellationToken cancellationToken)
        {
            // Mapping from AddStudentCommand to Student entity
            var studentMapper = _mapper.Map<Student>(request);
            // add student
            var result = await _studentService.CreateStudentAsync(studentMapper);
            if (result == "Success") return Created("Added successfully");
            else return BadRequest<string>();
        }

        public async Task<Response<string>> Handle(EditStudentCommand request, CancellationToken cancellationToken)
        {
            // Check If the Id is exist or not exist.
            var student = await _studentService.GetStudentByIdAsync(request.Id);
            if (student == null) return NotFound<string>("Student is Not found.");

            // Mapping from EditStudentCommand to Student entity
            var studentMapper = _mapper.Map<Student>(request);
            // update student
            var result = await _studentService.UpdateStudentAsync(studentMapper);
            if (result == "Success") return Success("Updated successfully");
            else return BadRequest<string>();
        }

        public async Task<Response<string>> Handle(DeleteStudentByIdCommand request, CancellationToken cancellationToken)
        {
            // Check If the Id is exist or not exist.
            var student = await _studentService.GetStudentByIdAsync(request.Id);
            if (student == null) return NotFound<string>("Student is Not found.");
            // delete student
            var result = await _studentService.DeleteStudentAsync(student);
            if (result == "Success") return Deleted<string>($"Deleted successfully {request.Id}");
            else return BadRequest<string>();
        }
        #endregion
    }
}
