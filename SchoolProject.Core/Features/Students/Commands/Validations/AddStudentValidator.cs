using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.Students.Commands.Models;
using SchoolProject.Core.Resources;
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
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructor
        public AddStudentValidator( IStudentService studentService, IStringLocalizer<SharedResources> stringLocalizer)
        {
            _studentService = studentService;
            _stringLocalizer = stringLocalizer;

            ApplyValidationRules();
            ApplyCustomValidations();

        }
        #endregion

        #region Actions
        public void ApplyValidationRules()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(100).WithMessage(_stringLocalizer[SharedResourcesKeys.MaximumLength]);
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(200).WithMessage(_stringLocalizer[SharedResourcesKeys.MaximumLength]);
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty]);
            RuleFor(x => x.DepartmentId)
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .GreaterThan(0).WithMessage(_stringLocalizer[SharedResourcesKeys.SelectValidOne]);
        }
        public void ApplyCustomValidations()
        {
            // Add any custom validation logic here if needed in the future.
            RuleFor(x => x.Name)
                .MustAsync(async (name, cancellationToken) => !await _studentService.IsNameExistExcludeSelf(name))
                .WithMessage(_stringLocalizer[SharedResourcesKeys.AlreadyExist]);
        }
        #endregion
    }
}
