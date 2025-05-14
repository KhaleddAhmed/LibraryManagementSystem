using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Core.Entities.Library
{
    public class Book : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string PublishedBy { get; set; }
        public DateOnly PublishedAt { get; set; }
        public int CategoryId { get; set; }
        public bool IsAvaliable { get; set; }
        public double BorrowPricePerDay { get; set; }
        public Category Category { get; set; }
        public virtual ICollection<UserBorrowing> UserBorrowings { get; set; }
    }
}
