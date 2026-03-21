using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Authorization.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Authorization.Commands.Handlers
{
    public class ClaimsCommandHandler : ResponseHandler,
        IRequestHandler<UpdateUserClaimsCommand, Response<string>>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IAuthorizationService _authorizationService;
        #endregion
        #region Constructor
        public ClaimsCommandHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                  IAuthorizationService authorizationService) : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _authorizationService = authorizationService;
        }
        #endregion
        #region Handle Functions
        public async Task<Response<string>> Handle(UpdateUserClaimsCommand request, CancellationToken cancellationToken)
        {
            // Pass the request directly to the service
            var result = await _authorizationService.UpdateUserClaimsAsync(request);

            switch (result)
            {
                case "UserIsNull":
                    return NotFound<string>(_stringLocalizer[SharedResourcesKeys.NotFound]);

                case "Success":
                    return Success<string>(_stringLocalizer[SharedResourcesKeys.Success]);

                case "FailedToAddNewClaims":
                case "FailedToRemoveOldClaims":
                    return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.UpdateFailed]);

                case "SystemError":
                    // Catch the new error from the database rollback!
                    return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.SystemError]);
                default:
                    return BadRequest<string>(result);
            }
        }
        #endregion
    }
}
