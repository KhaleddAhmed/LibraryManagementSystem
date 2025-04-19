using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Core.DTOs.Librarian
{
    public class ListOfLibrarianDto
    {
        public List<GetAllLibrarianDto> GetAllLibrarianDtos { get; set; }
        public int Count { get; set; }
    }
}
