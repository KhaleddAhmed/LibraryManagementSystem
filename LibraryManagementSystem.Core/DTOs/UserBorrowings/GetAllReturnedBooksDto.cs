using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Core.DTOs.UserBorrowings
{
    public class GetAllReturnedBooksDto
    {
        public string BorrowerId { get; set; }
        public string BookTitle { get; set; }
        public string Borrower { get; set; }
        public DateTime BorrowDate { get; set; }
    }
}
