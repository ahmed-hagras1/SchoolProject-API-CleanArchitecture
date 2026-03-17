using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly UserManager<User> _userManager;
        #endregion

        #region Constructor
        public AuthorizationService(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
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

        public async Task<string> DeleteRoleAsync(int id)
        {
            // 1. Find the role
            var role = await _roleManager.FindByIdAsync(id.ToString());

            // 2. If the role doesn't exist, return null early
            if (role == null)
            {
                return "NotFound";
            }
            bool isRoleAssignedToAnyUser = await _userManager.GetUsersInRoleAsync(role.Name).ContinueWith(task => task.Result.Any());
            if (isRoleAssignedToAnyUser)
            {
                return "RoleAssignedToUsers"; // We can catch this specific string in the Handler
            }

            // 💡 PRO-TIP: Protect your core system roles from accidental deletion!
            // You don't want another Admin to accidentally delete the "Admin" role and lock everyone out.
            if (role.Name == "Admin")
            {
                return "CannotDeleteSystemRole"; // We can catch this specific string in the Handler
            }

            // 3. Await the Delete properly
            var result = await _roleManager.DeleteAsync(role);

            // 4. Return a success indicator
            if (result.Succeeded)
            {
                return "Success";
            }

            return result.Errors.FirstOrDefault()?.Description ?? "Error"; // Return the first error description or a generic "Error" if none
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

        public async Task<Role> GetRoleById(int id) => await _roleManager.FindByIdAsync(id.ToString());

        public async Task<List<Role>> GetRolesAsync() => await _roleManager.Roles.ToListAsync();

        public async Task<bool> IsRoleExist(string roleName) => await _roleManager.RoleExistsAsync(roleName);

        public async Task<bool> IsRoleExist(int roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            return role != null;
        }
        #endregion
    }
}
