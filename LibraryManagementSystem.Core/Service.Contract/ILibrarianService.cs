using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Core.DTOs.Librarian;
using LibraryManagementSystem.Core.DTOs.UserBorrowings;
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
        Task<
            GenericResponse<List<GetAllBorrowRequestsFromMembersDto>>
        > GetAllBorrowRequestsFromMembersAsync();

        Task<GenericResponse<bool>> ApproveRequestedBorrowFromMemberAsync(
            string memberId,
            string BookTitle
        );

        Task<GenericResponse<bool>> RejectRequestedBorrowFromMemberAsync(
            string memberId,
            string BookTitle
        );
        Task<GenericResponse<List<GetAllReturnedBooksDto>>> GetAllReturnedBooksAsync();

        Task<GenericResponse<bool>> ApproveReturnedBook(string borrowerId, string bookTitle);
    }
}
