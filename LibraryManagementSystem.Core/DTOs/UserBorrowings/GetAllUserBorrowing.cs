using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Core.DTOs.UserBorrowings
{
    public class GetAllUserBorrowing
    {
        public string BookTitle { get; set; }
        public double Amount { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsAccepted { get; set; }
    }
}
