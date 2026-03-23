using SchoolProject.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Service.Abstracts
{
    public interface IApplicationUserService
    {
        Task <string> AddApplicationUserAsync(User user, string password);
    }
}
