using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagementSystem.Repository.Data.Seeding
{
    public static class RoleSeeding
    {
        public static async Task SeedRoleAsync(RoleManager<IdentityRole> _roleManager)
        {
            if (_roleManager.Roles.Count() == 0)
            {
                var Adminrole = new IdentityRole() { Name = "Admin" };

                var userRole = new IdentityRole() { Name = "User" };

                var librarianRole = new IdentityRole() { Name = "Librarian" };

                await _roleManager.CreateAsync(Adminrole);
                await _roleManager.CreateAsync(userRole);
                await _roleManager.CreateAsync(librarianRole);
            }
        }
    }
}
