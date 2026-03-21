using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Authorization.Queries.Models;
using SchoolProject.Core.Features.Authorization.Queries.Results;
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
    public class RoleQueryHandler : ResponseHandler,
        IRequestHandler<GetRolesListQuery, Response<List<GetRolesListResult>>>,
        IRequestHandler<GetRoleByIdQuery, Response<GetRoleResult>>,
        IRequestHandler<ManageUserRolesQuery, Response<ManageUserRolesResult>>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        #endregion
        #region Constructor
        public RoleQueryHandler(IStringLocalizer<SharedResources> stringLocalizer,
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
        public async Task<Response<List<GetRolesListResult>>> Handle(GetRolesListQuery request, CancellationToken cancellationToken)
        {
            var roles = await _authorizationService.GetRolesAsync();
            var result = _mapper.Map<List<GetRolesListResult>>(roles);

            return Success(result);
        }

        public async Task<Response<GetRoleResult>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var role = await _authorizationService.GetRoleById(request.Id);
            if (role == null)
                return NotFound<GetRoleResult>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            var result = _mapper.Map<GetRoleResult>(role);

            return Success(result);
        }
        public async Task<Response<ManageUserRolesResult>> Handle(ManageUserRolesQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
                return NotFound<ManageUserRolesResult>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            var result = await _authorizationService.GetManageUserRolesAsync(user);
            return Success(result);
        }
        #endregion
    }
}
