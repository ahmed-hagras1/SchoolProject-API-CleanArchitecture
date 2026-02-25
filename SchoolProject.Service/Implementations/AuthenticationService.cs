using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Helpers;
using SchoolProject.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Service.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Fields
        private readonly JWTSettings _jwtSettings;
        #endregion

        #region Constructor
        public AuthenticationService(IOptions<JWTSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }
        #endregion

        #region Methods
        public Task<string> GetJWTToken(User user)
        {
            // List of claims that will be included in the token, and these claims will be used to identify the user and his roles, and other information that you want to include in the token.
            // This is built-in Claims.
            //var claims = new List<Claim>()
            //{
            //    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            //    new Claim(ClaimTypes.Name, user.UserName),
            //    new Claim(ClaimTypes.Email, user.Email)
            //};

            // This is custom Claims, and you can make any claim you want, and you can use it in the future to identify the user and his roles, and other information that you want to include in the token.
            var claims = new List<Claim>
            {
                new Claim(nameof(UserClaimModel.UserName), user.UserName ?? string.Empty),
                new Claim(nameof(UserClaimModel.Email), user.Email ?? string.Empty),
                new Claim(nameof(UserClaimModel.PhoneNumber), user.PhoneNumber ?? string.Empty)
            };

            // Create the security key from your JWT settings
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var jwtToken = new JwtSecurityToken(_jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                null,
                DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature) // Corrected this line
                );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return Task.FromResult(accessToken);
        }
        #endregion
    }
}
