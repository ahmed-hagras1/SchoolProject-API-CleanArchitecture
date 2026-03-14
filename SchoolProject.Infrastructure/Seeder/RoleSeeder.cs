using Microsoft.AspNetCore.Identity;
using SchoolProject.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Infrastructure.Seeder
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(RoleManager<Role> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                var roles = new List<Role>
                    {
                        new Role { Name = "Admin" },
                        new Role { Name = "Instructor" },
                        new Role { Name = "Student" }
                    };

                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }
            }
        }
    }
}
