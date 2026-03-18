using SchoolProject.Data.DTOs;
using SchoolProject.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Service.Abstracts
{
    public interface IAuthorizationService
    {
        Task<string> AddRoleAsync(string roleName);
        //Task<IdentityResult> AssignRoleToUserAsync(string userName, string roleName);
        Task<bool> IsRoleExist(string roleName);
        Task<bool> IsRoleExist(int roleId);
        Task<string> EditRoleAsync(int id, string roleName);
        Task<string> DeleteRoleAsync(int id);
        Task<List<Role>> GetRolesAsync();
        Task<Role> GetRoleById(int id);
        Task<ManageUserRolesResultDTO> GetManageUserRolesAsync(User user);
        Task<string> UpdateUserRolesAsync(ManageUserRolesResultDTO request);
    }
}
