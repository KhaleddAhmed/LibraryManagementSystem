using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Core.Entities.User;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagementSystem.Repository.Data.Seeding
{
    public static class AdminDbSeeding
    {
        public static async Task SeedAdminAsync(UserManager<AppUser> userManager)
        {
            if (userManager.Users.Count() == 0)
            {
                var email = "Khaled.Ahmed@gmail.com";
                var admin = new AppUser()
                {
                    Email = email,
                    UserName = email.Split("@")[0],
                    FirstName = "Khaled",
                    LastName = "Ahmed",
                    Address = "12-Dokki-Giza-Egypt",
                    PhoneNumber = "01012313987",
                    IsAccepted = true,
                };

                await userManager.CreateAsync(admin, "P@ssw0rd");
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}
