using FluentValidation;
using SchoolProject.Core.Features.Students.Commands.Models;
using SchoolProject.Data.Entities;
using SchoolProject.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Students.Commands.Validations
{
    public class AddStudentValidator : AbstractValidator<AddStudentCommand>
    {
        #region Fields
        private readonly IStudentService _studentService;
        #endregion

        #region Constructor
        public AddStudentValidator( IStudentService studentService)
        {
            ApplyValidationRules();
            ApplyCustomValidations();

            _studentService = studentService;
        }
        #endregion

        #region Actions
        public void ApplyValidationRules()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Student name is required.")
                .MaximumLength(100).WithMessage("Student name must not exceed 100 characters.");
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(200).WithMessage("Address must not exceed 200 characters.");
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone number is required.");
            RuleFor(x => x.DepartmentId)
                .NotNull().WithMessage("Department Id is required.")
                .GreaterThan(0).WithMessage("Department Id must be a positive integer.");
        }
        public void ApplyCustomValidations()
        {
            // Add any custom validation logic here if needed in the future.
            RuleFor(x => x.Name)
                .MustAsync(async (name, cancellationToken) => !await _studentService.IsNameExist(name))
                .WithMessage("This student name is already taken.");
        }
        #endregion
    }
}
