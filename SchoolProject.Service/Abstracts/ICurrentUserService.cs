using SchoolProject.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Service.Abstracts
{
    public interface ICurrentUserService
    {
        int GetCurrentUserId();
        string GetUserEmail();
        bool IsAuthenticated();
        Task<User> GetCurrentUserAsync();
        Task<List<string>> GetCurrentUserRolesAsync();
    }
}
