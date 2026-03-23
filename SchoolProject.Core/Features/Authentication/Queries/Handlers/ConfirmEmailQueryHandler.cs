using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Authentication.Queries.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Authentication.Queries.Handlers
{
    public class ConfirmEmailQueryHandler : ResponseHandler, IRequestHandler<ConfirmEmailQuery, Response<string>>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;

        public ConfirmEmailQueryHandler(IAuthenticationService authenticationService, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _authenticationService = authenticationService;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<Response<string>> Handle(ConfirmEmailQuery request, CancellationToken cancellationToken)
        {
            // هنا نقوم باستدعاء الدالة من الـ Service!
            var confirmResult = await _authenticationService.ConfirmEmailAsync(request.UserId, request.Code);

            if (confirmResult == "Success")
            {
                return Success<string>("Email confirmed successfully! You can now log in.");
            }
            else if (confirmResult == "UserNotFound")
            {
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.UserNotFound]);
            }
            else
            {
                return BadRequest<string>("Failed to confirm email. The link may be invalid or expired.");
            }
        }
    }
}
