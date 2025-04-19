using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Core.Entities.Library;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagementSystem.Core.Entities.User
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public bool IsAccepted { get; set; } = false;
        public virtual ICollection<UserBorrowing> UserBorrowings { get; set; }
    }
}
