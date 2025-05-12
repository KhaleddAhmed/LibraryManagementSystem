using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Core.Responses;

namespace LibraryManagementSystem.Core.Service.Contract
{
    public interface IAdminService
    {
        Task<GenericResponse<bool>> AcceptUserAsync(string userId);
        Task<GenericResponse<bool>> RejectUserAsync(string userId);
    }
}
