using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.Authorization.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Authorization.Commands.Validations
{
    public class EditRoleValidator : AbstractValidator<EditRoleCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IAuthorizationService _authorizationService;
        #endregion

        #region Constructor
        public EditRoleValidator(IStringLocalizer<SharedResources> stringLocalizer, IAuthorizationService authorizationService)
        {
            _stringLocalizer = stringLocalizer;
            _authorizationService = authorizationService;

            // If a rule fails, stop immediately. Don't hit the DB if the string is empty!
            RuleLevelCascadeMode = CascadeMode.Stop;

            ApplyValidationsRules();
            ApplyCustomValidations();
        }
        #endregion

        #region Actions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.Id)
                 .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                 .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required]);

            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                            .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required]);
        }
        public void ApplyCustomValidations()
        {


            RuleFor(x => x.RoleName)
                            .MustAsync(async (roleName, cancellationToken) => !await _authorizationService.IsRoleExist(roleName)).WithMessage(_stringLocalizer[SharedResourcesKeys.AlreadyExist]);
                            
        }
        #endregion
    }
}
