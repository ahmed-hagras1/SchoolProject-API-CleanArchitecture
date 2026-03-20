using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Helpers;
using SchoolProject.Data.Requests;
using SchoolProject.Data.Results;
using SchoolProject.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        public async Task<ManageUserRolesResult> GetManageUserRolesAsync(User user)
        {
            // 1. Initialize the DTO and set the UserId
            var response = new ManageUserRolesResult
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

        public async Task<string> UpdateUserRolesAsync(ManageUserRolesResult request)
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

        public async Task<ManageUserClaimsResult> GetManageUserClaimsDataAsync(User user)
        {
            // 1. Initialize the Result object
            var response = new ManageUserClaimsResult
            {
                UserId = user.Id,
                UserClaims = new List<UserClaim>()
            };

            // 2. Get the user's currently assigned claims from the database
            var userClaims = await _userManager.GetClaimsAsync(user);

            // 3. Loop through the universal ClaimsStore list
            foreach (var claim in ClaimsStore.Claims)
            {
                var userClaim = new UserClaim
                {
                    Type = claim.Type, // e.g., "Create Student"
                                       // Check if the user has this exact claim type assigned to them
                    Value = userClaims.Any(x => x.Type == claim.Type)
                };

                response.UserClaims.Add(userClaim);
            }

            return response;
        }

        public async Task<string> UpdateUserClaimsAsync(UpdateUserClaimsRequest request)
        {
            // 1. Find the user
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                return "UserIsNull";
            }

            // 2. Get the user's current claims from the database (These are actual Claim objects)
            var currentClaims = await _userManager.GetClaimsAsync(user);

            // 3. Get the NEW claim TYPES the user selected (where Value/HasClaim is true)
            var selectedClaimTypes = request.UserClaims.Where(x => x.Value).Select(x => x.Type).ToList();

            // Get the CURRENT claim TYPES as a list of strings so we can compare them
            var currentClaimTypes = currentClaims.Select(x => x.Type).ToList();

            // 4. Calculate the "Diff" (What to add and what to remove)
            var claimTypesToAdd = selectedClaimTypes.Except(currentClaimTypes).ToList();
            var claimTypesToRemove = currentClaimTypes.Except(selectedClaimTypes).ToList();

            // 5. Convert the string types back into actual Claim objects for Entity Framework
            // For removal: Find the exact existing claim objects that match the types we want to remove
            var claimsToRemove = currentClaims.Where(x => claimTypesToRemove.Contains(x.Type)).ToList();

            // For addition: Create brand new claim objects. 
            // (We set the value to "true" to represent they have the permission)
            var claimsToAdd = claimTypesToAdd.Select(x => new Claim(x, "true")).ToList();

            // 6. Apply the changes safely within a Transaction
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    // Remove old claims
                    if (claimsToRemove.Any())
                    {
                        var removeResult = await _userManager.RemoveClaimsAsync(user, claimsToRemove);
                        if (!removeResult.Succeeded)
                            return "FailedToRemoveOldClaims"; // Rolls back automatically
                    }

                    // Add new claims
                    if (claimsToAdd.Any())
                    {
                        var addResult = await _userManager.AddClaimsAsync(user, claimsToAdd);
                        if (!addResult.Succeeded)
                            return "FailedToAddNewClaims"; // Rolls back automatically
                    }

                    // Commit to the database
                    transaction.Complete();
                    return "Success";
                }
                catch (Exception ex)
                {
                    // TODO: Log the exception (ex.Message)
                    return "SystemError";
                }
            }
        }
        #endregion
    }
}
