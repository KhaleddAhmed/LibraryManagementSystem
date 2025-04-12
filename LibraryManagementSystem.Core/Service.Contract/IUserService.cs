using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Core.DTOs.User;
using LibraryManagementSystem.Core.Responses;

namespace LibraryManagementSystem.Core.Service.Contract
{
    public interface IUserService
    {
        Task<GenericResponse<UserDto>> LoginAsync(LoginDto loginDto);
        Task<GenericResponse<UserDto>> RegisterAsync(RegisterDto registerDto);

        Task<bool> CheckEmailExistAsync(string email);
    }
}
