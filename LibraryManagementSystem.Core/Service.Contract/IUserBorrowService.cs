using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Core.DTOs.UserBorrowings;
using LibraryManagementSystem.Core.Responses;

namespace LibraryManagementSystem.Core.Service.Contract
{
    public interface IUserBorrowService
    {
        Task<GenericResponse<bool>> CreateBorrowAsync(CreateBorrowDto createBorrowDto);
        Task<GenericResponse<List<GetAllUserBorrowing>>> GetAllUserBorrowingAsync(string userId);
    }
}
