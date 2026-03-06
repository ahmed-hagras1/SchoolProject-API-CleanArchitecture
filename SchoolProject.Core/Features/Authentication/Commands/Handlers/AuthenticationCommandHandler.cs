using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.ApplicationUser.Commands.Models;
using SchoolProject.Core.Features.Authentication.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Helpers;
using SchoolProject.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAuthenticationService = SchoolProject.Service.Abstracts.IAuthenticationService;

namespace SchoolProject.Core.Features.Authentication.Commands.Handlers
{
    public class AuthenticationCommandHandler : ResponseHandler,
        IRequestHandler<SignInCommand, Response<JWTAuthResult>>,
        IRequestHandler<RefreshTokenCommand, Response<JWTAuthResult>>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IAuthenticationService _authenticationService;
        #endregion

        #region Constructor
        public AuthenticationCommandHandler(IStringLocalizer<SharedResources> stringLocalizer,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IAuthenticationService authenticationService) : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationService = authenticationService;
        }
        #endregion
        #region Handle Functions
        public async Task<Response<JWTAuthResult>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            // Check if the user exists, or not
            var user = await _userManager.FindByNameAsync(request.UserName);

            if(user == null) return NotFound<JWTAuthResult>(_stringLocalizer[SharedResourcesKeys.UserNotFound]);
            // Check if the password is correct, or not
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (!result.Succeeded) return BadRequest<JWTAuthResult>(_stringLocalizer[SharedResourcesKeys.InvalidPassword]);

            // If the user exists and the password is correct, return token.
            // Generate Token.
            var jwtResult = await _authenticationService.GetJWTToken(user);

            // Return Token
            return Success(jwtResult);
        }

        public async Task<Response<JWTAuthResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // 1. Try to process the tokens
                var result = await _authenticationService.GetRefreshToken(request.AccessToken, request.RefreshToken);

                // 2. If successful, return the new tokens
                return Success(result);
            }
            catch (SecurityTokenException ex)
            {
                var localizedMessage = _stringLocalizer[ex.Message];

                // 3. If validation fails (expired, revoked, invalid signature), catch it safely.
                // Use BadRequest or Unauthorized (depending on what your ResponseHandler supports).
                switch (ex.Message)
                {
                    case SharedResourcesKeys.RefreshTokenRevoked:
                    case SharedResourcesKeys.RefreshTokenExpired:
                    case SharedResourcesKeys.RefreshTokenNotFound:
                    case SharedResourcesKeys.UserNotFound:
                    case SharedResourcesKeys.TokenClaimsMissing:
                    case SharedResourcesKeys.TokenIsInvalid:
                    case SharedResourcesKeys.AlgorithmIsInvalid:
                        return Unauthorized<JWTAuthResult>(localizedMessage);
                    default:
                        return BadRequest<JWTAuthResult>(ex.Message);
                }
            }
            catch (Exception ex)
            {
                // 4. Catch any other unexpected system errors
                return BadRequest<JWTAuthResult>("An error occurred while refreshing the token: " + ex.Message);
            }
        }
        #endregion
    }
}
