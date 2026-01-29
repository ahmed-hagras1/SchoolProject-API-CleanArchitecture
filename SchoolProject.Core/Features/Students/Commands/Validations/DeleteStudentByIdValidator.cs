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
    public class DeleteStudentByIdValidator : AbstractValidator<DeleteStudentByIdCommand>
    {
        #region Fields
        private readonly IStudentService _studentService;
        #endregion

        #region Constructor
        public DeleteStudentByIdValidator(IStudentService studentService)
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
        }
        public void ApplyCustomValidations()
        {
            // Add any custom validation logic here if needed in the future.
        }
        #endregion
    }
}
