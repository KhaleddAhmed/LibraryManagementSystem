using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Core.DTOs.UserBorrowings
{
    public class GetAllBorrowRequestsFromMembersDto
    {
        public string BorrowerId { get; set; }
        public string NameMember { get; set; }
        public string NameBook { get; set; }
        public string Category { get; set; }
    }
}
