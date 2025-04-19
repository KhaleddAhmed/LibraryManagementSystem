using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Core.DTOs.Librarian;
using LibraryManagementSystem.Core.Responses;

namespace LibraryManagementSystem.Core.Service.Contract
{
    public interface ILibrarianService
    {
        Task<GenericResponse<bool>> CreateLibrarianAsync(CreateLibrarianDto createLibrarianDto);
        Task<GenericResponse<bool>> UpdateLibrarianAsync(UpdateLibrarianDto updateLibrarianDto);
        Task<GenericResponse<bool>> DeleteLibrarianAsync(string id);
        Task<GenericResponse<ListOfLibrarianDto>> GetAllLibrarianAsync();
        Task<GenericResponse<GetLibrarianDto>> GetLibrarianAsync(string librarianId);
    }
}
