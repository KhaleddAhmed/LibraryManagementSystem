using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Core.Entities.Library;

namespace LibraryManagementSystem.Core.DTOs.UserBorrowings
{
    public class GetBorrowingsAdminDto
    {
        public string BookTitle { get; set; }
        public string Borrower { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }

        public BorrowStatus BorrowStatus { get; set; }
    }
}
