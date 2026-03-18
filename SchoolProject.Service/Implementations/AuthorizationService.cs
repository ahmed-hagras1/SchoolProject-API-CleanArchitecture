using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.DTOs;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

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
        public async Task<ManageUserRolesResultDTO> GetManageUserRolesAsync(User user)
        {
            // 1. Initialize the DTO and set the UserId
            var response = new ManageUserRolesResultDTO
            {
                UserId = user.Id,
                Roles = new List<RoleResult>() // Initialize the list so we don't get a NullReferenceException
            };

            // 2. Get All Roles from the database
            var roles = await _roleManager.Roles.ToListAsync();

            // 3. Get the roles assigned to this specific user (This returns a List of strings: the role names)
            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                response.Roles.Add(new RoleResult
                {
                    Id = role.Id,
                    Name = role.Name,
                    HasRole = userRoles.Contains(role.Name) // Check if the user's roles contain this role's name
                });
            }
            return response;
        }

        public async Task<string> UpdateUserRolesAsync(ManageUserRolesResultDTO request)
        {
            // 1. Find the user
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                return "UserIsNull";
            }

            // 2. Get the user's current roles from the database
            var currentRoles = await _userManager.GetRolesAsync(user);

            // 3. Get the new roles the user selected from the request (where HasRole is true)
            var selectedRoles = request.Roles.Where(x => x.HasRole).Select(x => x.Name).ToList();

            // 4. Calculate the "Diff" (What to add and what to remove)
            var rolesToAdd = selectedRoles.Except(currentRoles);    // Exists in selected, but not in current
            var rolesToRemove = currentRoles.Except(selectedRoles); // Exists in current, but not in selected


            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    // 2. Try to remove roles
                    if (rolesToRemove.Any())
                    {
                        var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                        if (!removeResult.Succeeded)
                            return "FailedToRemoveOldRoles"; // Rolls back automatically
                    }
                    

                    // 3. Try to add roles
                    if (rolesToAdd.Any())
                    {
                        var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
                        if (!addResult.Succeeded)
                            return "FailedToAddNewRoles"; // Rolls back automatically
                    }

                    // 4. Everything worked! Commit to the database.
                    transaction.Complete();
                    return "Success";
                }
                catch (Exception ex)
                {
                    // An unexpected error happened (e.g., database connection lost).
                    // The code exited before transaction.Complete(), so the rollback already happened!

                    // TODO: Log the exception (ex.Message) here if you have a logger like Serilog

                    return "SystemError"; // Return a clean error string for your Handler to catch
                }
            }
        }
        #endregion
    }
}
