using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Authorization.Commands.Models;
using SchoolProject.Core.Features.Email.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Email.Commands.Handlers
{
    public class EmailCommandHandler : ResponseHandler,
        IRequestHandler<SendEmailCommand, Response<string>>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IEmailService _emailService;
        #endregion
        #region Constructor
        public EmailCommandHandler(IStringLocalizer<SharedResources> stringLocalizer, IEmailService emailService) : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _emailService = emailService;
        }
        #endregion
        #region Handle Functions
        public async Task<Response<string>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            // 3. Call the service
            var result = await _emailService.SendEmail(request.Email, request.Message);

            if (result == "Success")
            {
                return Success<string>("Email sent successfully!");
            }

            return BadRequest<string>("Failed to send the email.");
        }
        #endregion
    }
}
