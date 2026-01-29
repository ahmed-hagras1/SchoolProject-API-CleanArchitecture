using FluentValidation;
using SchoolProject.Core.Features.Students.Commands.Models;
using SchoolProject.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Students.Commands.Validations
{
    public class EditStudentValidator : AbstractValidator<EditStudentCommand>
    {
        #region Fields
        private readonly IStudentService _studentService;
        #endregion

        #region Constructor
        public EditStudentValidator(IStudentService studentService)
        {
            ApplyValidationRules();
            ApplyCustomValidations();

            _studentService = studentService;
        }
        #endregion

        #region Actions
        public void ApplyValidationRules()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Student Id is required.")
                .GreaterThan(0).WithMessage("Student Id must be a positive integer.");
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
            RuleFor(x => x.Id)
               .MustAsync(async (id, cancellationToken) => await _studentService.IsStudentIdExist(id))
               .WithMessage("This student not exist.");

            RuleFor(x => x.Name)
                .MustAsync(async (model, key, cancellationToken) => !await _studentService.IsNameExistExcludeSelf(key,model.Id))
                .WithMessage("This student name is already taken.");


        }
        #endregion
    }
}
