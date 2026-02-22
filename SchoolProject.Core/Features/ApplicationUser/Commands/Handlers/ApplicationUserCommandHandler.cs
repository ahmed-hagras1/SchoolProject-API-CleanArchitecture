using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.ApplicationUser.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Data.Entities.Identity;
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
        #endregion

        #region Constructor
        public ApplicationUserCommandHandler(IMapper mapper,IStringLocalizer<SharedResources> stringLocalizer, UserManager<User> userManager) : base(stringLocalizer)
        {
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
            _userManager = userManager;
        }


        #endregion

        #region Handle Method
        public async Task<Response<string>> Handle(AddApplicationUserCommand request, CancellationToken cancellationToken)
        {
            // Check If the Email is exist or not exist.
            var user = await _userManager.FindByEmailAsync(request.Email);
            // Do not execute the rules inside these brackets => {} unless Email is not null.
            if (user != null) return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.EmailIsAlreadyExist]);

            // Note => You can make other validations like check if the phone number, or UserName is exist or not exist, but I will not make it because I will make it in the future when I will make Edit operation for user.
            var userByUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userByUserName != null) return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.AlreadyExist]);

            // Mapping from AddApplicationUserCommand to User entity, and then add user using UserManager.
            var applicationUserMapper = _mapper.Map<User>(request);
            var result = await _userManager.CreateAsync(applicationUserMapper, request.Password);
            if (result.Succeeded) return Created("Added successfully");
            // else return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedToAddUser]);
            else return BadRequest<string>(result.Errors.FirstOrDefault().Description);
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
