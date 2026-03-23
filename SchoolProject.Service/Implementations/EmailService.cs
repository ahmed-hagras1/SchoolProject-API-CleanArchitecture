using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using MimeKit;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Helpers;
using SchoolProject.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Service.Implementations
{
    public class EmailService : IEmailService
    {
        #region Fields
        private readonly EmailSettings _emailSettings;
        #endregion

        #region Constractor
        // 1. Inject the settings into the Constructor
        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        #endregion

        #region Methods
        public async Task<string> SendEmailAsync(string email, string message)
        {
            try
            {
                var emailMessage = new MimeMessage();

                // 2. Use the variables instead of hardcoded strings
                emailMessage.From.Add(new MailboxAddress("School Project", _emailSettings.Email));
                emailMessage.To.Add(new MailboxAddress("User", email));
                emailMessage.Subject = "Notification from School Project";
                emailMessage.Body = new TextPart("plain") { Text = message };

                using (var client = new SmtpClient())
                {
                    // 3. Use Host and Port from settings
                    await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);

                    // 4. Use Email and Password from settings
                    await client.AuthenticateAsync(_emailSettings.Email, _emailSettings.Password);

                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }

                return "Success";
            }
            catch (Exception ex)
            {
                return "Failed";
            }
        }
        #endregion
    }
}
