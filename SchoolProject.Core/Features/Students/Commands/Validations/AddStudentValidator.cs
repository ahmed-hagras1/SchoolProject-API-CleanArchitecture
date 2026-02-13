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
        private readonly IDepartmentService _departmentService;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructor
        public AddStudentValidator( IStudentService studentService, IDepartmentService departmentService, IStringLocalizer<SharedResources> stringLocalizer)
        {
            _studentService = studentService;
            _departmentService = departmentService;
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

            // Do not execute the rules inside these brackets => {} unless DepartmentId is not null.
            When(d => d.DepartmentId != null, () =>
            {
                RuleFor(x => x.DepartmentId)
                    .MustAsync(async (deptId, cancellationToken) => await _departmentService.IsDepartmentExist(deptId))
                    .WithMessage(_stringLocalizer[SharedResourcesKeys.DepartmentIsNotExist]);
            });
        }
        #endregion
    }
}
