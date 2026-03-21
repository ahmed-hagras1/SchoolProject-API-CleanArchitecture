using Microsoft.AspNetCore.Identity;
using SchoolProject.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Infrastructure.Seeder
{
    public static class UserSeeder
    {
        public static async Task SeedAsync(UserManager<User> userManager)
        {
            if (!userManager.Users.Any())
            {
                var defaultUser = new User()
                {
                    UserName = "Ahmed",
                    Email = "ahmed@gmail.com",
                    FullName = "Ahmed Hagras",
                    Country = "Egypt",
                    PhoneNumber = "1234",
                    Address = "Tanta",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

                await userManager.CreateAsync(defaultUser, "Ahmed1@");
                await userManager.AddToRoleAsync(defaultUser, "Admin");
            }
        }
    }
}
