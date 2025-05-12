using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Core.DTOs.Book;
using LibraryManagementSystem.Core.Responses;

namespace LibraryManagementSystem.Core.Service.Contract
{
    public interface IBookService
    {
        Task<GenericResponse<bool>> CreateBookAsync(CreateBookDto createBookDto, string userName);
        Task<GenericResponse<bool>> DeleteBookAsync(int id);
        Task<GenericResponse<bool>> UpdateBookAsync(UpdateBookDto updateBookDto, string userName);
        Task<GenericResponse<List<GetAllBooksDto>>> GetAllBooksAsync(int? categoryId);
        Task<GenericResponse<GetBookDto>> GetBookAsync(int id);
    }
}
