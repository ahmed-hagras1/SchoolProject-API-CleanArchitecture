using Microsoft.AspNetCore.Identity;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Service.Implementations
{
    public class AuthorizationService : IAuthorizationService
    {
        #region Fields
        private readonly RoleManager<Role> _roleManager;
        #endregion

        #region Constructor
        public AuthorizationService(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }
        #endregion

        #region Methods
        // IdentityResult
        public async Task<string> AddRoleAsync(string roleName)
        {
            var result = await _roleManager.CreateAsync(new Role { Name = roleName });
            if (result.Succeeded)
                return roleName;
            else
                return null;
        }
        public async Task<bool> IsRoleExist(string roleName) => await _roleManager.RoleExistsAsync(roleName);
        #endregion
    }
}
