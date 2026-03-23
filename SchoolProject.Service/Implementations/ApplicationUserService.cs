using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Org.BouncyCastle.Asn1.Ocsp;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Service.Implementations
{
    public class ApplicationUserService : IApplicationUserService
    {

        #region Fields
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        #endregion

        #region Constractor
        public ApplicationUserService(UserManager<User> userManager , IHttpContextAccessor httpContextAccessor, IEmailService emailService)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
        }
        #endregion

        #region Methods
        public async Task<string> AddApplicationUserAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // If you want to assign a specific role to the user, you can do it here. For example, if you want to assign the "User" role to every new user, you can uncomment the following line and make sure that the "User" role exists in your system.
                // await _userManager.AddToRoleAsync(applicationUserMapper, request.Role);

                // If there is no users in the database, make the first user an admin, otherwise make it a normal user.
                //if (_userManager.Users.Any())
                //    await _userManager.AddToRoleAsync(applicationUserMapper, "User");
                //else
                //    await _userManager.AddToRoleAsync(applicationUserMapper, "Admin");

                await _userManager.AddToRoleAsync(user, "Admin");

                // 2. Generate the Confirmation Token
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                // 3. Encode the code so special characters (like + or /) don't break the URL
                // (Requires: using Microsoft.AspNetCore.WebUtilities; and using System.Text;)
                var encodedCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var requestAccessor = _httpContextAccessor.HttpContext.Request;

                // Using String Interpolation makes the URL much easier to read!
                var returnUrl = $"{requestAccessor.Scheme}://{requestAccessor.Host}/Api/V1/Authentication/ConfirmEmail?userId={user.Id}&code={encodedCode}";

                // 5. Create the message and send it!
                var message = $"Welcome to School Project! Please confirm your email by clicking this link: <a href='{returnUrl}'>Click Here</a>";

                var sendEmailResult = await _emailService.SendEmailAsync(user.Email, message);

                if (sendEmailResult == "Success")
                {
                    return "Success";
                }
                else
                {
                    // The email failed, so roll back the user creation
                    await _userManager.DeleteAsync(user);
                    return "Failed";
                }


            }
            // else return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedToAddUser]);
            else return "Failed";
        }
        public async Task<string> ResendConfirmEmailAsync(string email)
        {
            // 1. البحث عن المستخدم
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return "UserNotFound";

            // 2. التحقق مما إذا كان مؤكداً بالفعل (لا نريد إرسال إيميل لشخص حسابه مفعل!)
            if (user.EmailConfirmed)
                return "AlreadyConfirmed";

            // 3. إنشاء رمز أمان جديد وبناء الرابط
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var requestAccessor = _httpContextAccessor.HttpContext.Request;
            var returnUrl = $"{requestAccessor.Scheme}://{requestAccessor.Host}/Api/V1/Authentication/ConfirmEmail?userId={user.Id}&code={encodedCode}";

            // 4. صياغة الرسالة الجديدة وإرسالها
            var message = $"Welcome back to School Project! Please confirm your email by clicking this new link: <a href='{returnUrl}'>Click Here</a>";
            var sendEmailResult = await _emailService.SendEmailAsync(user.Email, message);

            if (sendEmailResult == "Success")
                return "Success";
            else
                return "FailedToSendEmail";
        }
        #endregion
    }
}
