using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Core.DTOs.Book
{
    public class GetBookDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string PublishedBy { get; set; }
        public DateOnly PublishedAt { get; set; }
        public string CategoryName { get; set; }
    }
}
