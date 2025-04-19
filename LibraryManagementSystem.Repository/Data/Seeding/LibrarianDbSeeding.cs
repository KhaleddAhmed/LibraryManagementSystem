using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Core.Entities.User;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagementSystem.Repository.Data.Seeding
{
    public static class LibrarianDbSeeding
    {
        public static async Task LibrarianDbSeedAsync(UserManager<AppUser> userManager)
        {
            if (
                userManager.Users.Count() == 1
                && userManager.Users.Any(u => u.Email == "Khaled.Ahmed@gmail.com")
            )
            {
                var email = "Tamer.Elgiar@gmail.com";
                var librirain = new AppUser()
                {
                    Email = email,
                    UserName = email.Split("@")[0],
                    FirstName = "Tamer",
                    LastName = "Elgyar",
                    Address = "10-Agouza-Giza-Egypt",
                    PhoneNumber = "01012313987",
                    IsAccepted = true,
                };

                await userManager.CreateAsync(librirain, "P@ssw0rd");
                await userManager.AddToRoleAsync(librirain, "Librarian");
            }
        }
    }
}
