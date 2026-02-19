using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.ApplicationUser.Commands.Models;
using SchoolProject.Core.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.ApplicationUser.Commands.Validations
{
    public class AddApplicationUserValidator : AbstractValidator<AddApplicationUserCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructor
        public AddApplicationUserValidator(IStringLocalizer<SharedResources> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            ApplyValidationRules();
            ApplyCustomValidations();
        }
        #endregion

        #region Methods
        public void ApplyValidationRules()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(100).WithMessage(_stringLocalizer[SharedResourcesKeys.MaximumLength]);
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(50).WithMessage(_stringLocalizer[SharedResourcesKeys.MaximumLength]);
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .EmailAddress().WithMessage(_stringLocalizer[SharedResourcesKeys.InvalidEmail]);
            RuleFor(x => x.Address)
                .MaximumLength(200).WithMessage(_stringLocalizer[SharedResourcesKeys.MaximumLength]);
            RuleFor(x => x.Country)
                .MaximumLength(50).WithMessage(_stringLocalizer[SharedResourcesKeys.MaximumLength]);
            RuleFor(x => x.PhoneNumber)
                .MaximumLength(20).WithMessage(_stringLocalizer[SharedResourcesKeys.MaximumLength]);
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .MinimumLength(6).WithMessage(_stringLocalizer[SharedResourcesKeys.MinimumLength]);
            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .Equal(x => x.Password).WithMessage(_stringLocalizer[SharedResourcesKeys.PasswordsDoNotMatch]);
        }
        public void ApplyCustomValidations()
        {
            // Add any custom validation logic here if needed in the future.
        }
        #endregion
    }
}
