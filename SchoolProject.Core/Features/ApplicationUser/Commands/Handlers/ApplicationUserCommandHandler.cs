using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.AppMetaData;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.ApplicationUser.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.ApplicationUser.Commands.Handlers
{
    public class ApplicationUserCommandHandler : ResponseHandler,
        IRequestHandler<AddApplicationUserCommand, Response<string>>,
        IRequestHandler<UpdateApplicationUserCommand, Response<string>>,
        IRequestHandler<DeleteApplicationUserCommand, Response<string>>,
        IRequestHandler<ChangeUserPasswordCommand, Response<string>>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly IApplicationUserService _applicationUserService;
        #endregion

        #region Constructor
        public ApplicationUserCommandHandler(IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer,
            UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor,
            IEmailService emailService,
            IApplicationUserService applicationUserService) : base(stringLocalizer)
        {
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _applicationUserService = applicationUserService;
        }


        #endregion

        #region Handle Method
        public async Task<Response<string>> Handle(AddApplicationUserCommand request, CancellationToken cancellationToken)
        {
            // 1. Validations (Keep these here, it's good practice to validate before calling the service)
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null) return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.EmailIsAlreadyExist]);

            var userByUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userByUserName != null) return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.AlreadyExist]);

            // 2. Mapping
            var applicationUserMapper = _mapper.Map<User>(request);

            // 3. Call the Orchestrator Service! (This does all the database, token, and email work)
            var result = await _applicationUserService.AddApplicationUserAsync(applicationUserMapper, request.Password);

            // 4. Handle the results based on what the service returns
            if (result == "Success")
            {
                return Created("Added successfully. Please check your email to confirm your account.");
            }
            else if (result == "FailedToSendEmail") // The specific error we created in the service
            {
                return BadRequest<string>("User was created but failed to send confirmation email. Please try registering again later.");
            }
            else
            {
                return BadRequest<string>("Failed to add user.");
            }
        }
        public async Task<Response<string>> Handle(UpdateApplicationUserCommand request, CancellationToken cancellationToken)
        {
            // Check if user is exist.
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == request.Id);
            // Not Fount.
            if (user == null) return NotFound<string>(_stringLocalizer[SharedResourcesKeys.NotFound]);
            // Mapping.
           _mapper.Map(request,user);
            // Update.
            var result = await _userManager.UpdateAsync(user);
            // Check if it failed.
            if(!result.Succeeded) return BadRequest<string>(_stringLocalizer.GetString(SharedResourcesKeys.UpdateFailed));
            // Message.
            return Success<string>(_stringLocalizer[SharedResourcesKeys.Success]);
        }

        public async Task<Response<string>> Handle(DeleteApplicationUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (user == null) return NotFound<string>(_stringLocalizer[SharedResourcesKeys.NotFound]);
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded) return BadRequest<string>(_stringLocalizer.GetString(SharedResourcesKeys.DeleteFailed));
            return Deleted<string>(_stringLocalizer[SharedResourcesKeys.Deleted]);
        }

        public async Task<Response<string>> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (user == null) return NotFound<string>(_stringLocalizer[SharedResourcesKeys.NotFound]);
            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            // Optional: If you want to return the exact Identity errors (like "Password requires an uppercase letter")
            // var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            // return BadRequest<string>(errors);
            if (!result.Succeeded) return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.ChangePasswordFailed]);
            return Success<string>(_stringLocalizer[SharedResourcesKeys.Success]);
        }
        #endregion

    }
}
