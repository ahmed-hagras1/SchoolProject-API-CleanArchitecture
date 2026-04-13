using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Service.Implementations
{
    public class CurrentUserService : ICurrentUserService
    {
        #region Fields
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        #endregion
        #region Constructors
        public CurrentUserService(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }


        #endregion
        #region Methods
        public string GetUserEmail()
        {
            return _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
        }

        public int GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out int userId) ? userId : 0;
        }

        public bool IsAuthenticated()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        }
        public async Task<User> GetCurrentUserAsync()
        {
            var userId = GetCurrentUserId();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            return user;
        }

        public async Task<List<string>> GetCurrentUserRolesAsync()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                return roles.ToList();
            }

            return new List<string>();
        }
        #endregion
    }
}
