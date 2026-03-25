using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Service.Abstracts
{
    public interface IAuthenticationService
    {
        public Task<JWTAuthResult> GetJWTToken(User user);
        public Task<JWTAuthResult> GetRefreshToken(string accessToken, string refreshToken);
        public Task<string> ValidateToken(string accessToken);
        Task<string> RevokeRefreshToken(string accessToken);
        Task<string> ConfirmEmailAsync(int userId, string code);
        Task<string> ResetPasswordAsync(string email, string code, string newPassword);
    }
}
