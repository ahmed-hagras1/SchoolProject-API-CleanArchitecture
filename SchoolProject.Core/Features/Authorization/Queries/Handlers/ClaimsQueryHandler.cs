using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Authorization.Queries.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Results;
using SchoolProject.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Authorization.Queries.Handlers
{
    public class ClaimsQueryHandler : ResponseHandler,
        IRequestHandler<ManageUserClaimsQuery,Response<ManageUserClaimsResult>>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        #endregion
        #region Constructor
        public ClaimsQueryHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                  IAuthorizationService authorizationService,
                                  IMapper mapper,
                                  UserManager<User> userManager) : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _authorizationService = authorizationService;
            _mapper = mapper;
            _userManager = userManager;
        }
        #endregion
        #region Handle Functions
        public async Task<Response<ManageUserClaimsResult>> Handle(ManageUserClaimsQuery request, CancellationToken cancellationToken)
        {
            // 1. Check if the user actually exists
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (user == null)
            {
                return NotFound<ManageUserClaimsResult>(_stringLocalizer[SharedResourcesKeys.NotFound]);
            }

            // 2. Pass the user to the service to calculate their claims
            var result = await _authorizationService.GetManageUserClaimsDataAsync(user);

            // 3. Return the populated DTO back to the API
            return Success(result);
        }
        #endregion
    }
}
