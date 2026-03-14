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

        public async Task<string> EditRoleAsync(int id, string roleName)
        {
            // 1. Use the built-in async method instead of FirstOrDefault
            var role = await _roleManager.FindByIdAsync(id.ToString());

            // 2. If the role doesn't exist, return null early
            if (role == null)
            {
                return null;
            }

            // 3. Update the name
            role.Name = roleName;

            // 4. Await the update properly (NEVER use .Result)
            var result = await _roleManager.UpdateAsync(role);

            // 5. Return the string directly (no Task.FromResult needed)
            if (result.Succeeded)
            {
                return role.Name;
            }

            return null;
        }

        public async Task<bool> IsRoleExist(string roleName) => await _roleManager.RoleExistsAsync(roleName);
        #endregion
    }
}
