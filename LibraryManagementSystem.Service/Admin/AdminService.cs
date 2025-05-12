using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Core;
using LibraryManagementSystem.Core.Entities.User;
using LibraryManagementSystem.Core.Responses;
using LibraryManagementSystem.Core.Service.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagementSystem.Service.Admin
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public AdminService(UserManager<AppUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse<bool>> AcceptUserAsync(string userId)
        {
            var genericResponse = new GenericResponse<bool>();
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                genericResponse.Message = "Invalid user to accept ";

                return genericResponse;
            }
            user.IsAccepted = true;
            _unitOfWork.Repository<AppUser, string>().Update(user);
            var result = await _unitOfWork.CompleteAsync();
            if (result > 0)
            {
                genericResponse.StatusCode = StatusCodes.Status200OK;
                genericResponse.Message = "Success to accept user";
                genericResponse.Data = true;
                return genericResponse;
            }

            genericResponse.StatusCode = StatusCodes.Status200OK;
            genericResponse.Message = "Failed to accept user";
            return genericResponse;
        }

        public async Task<GenericResponse<bool>> RejectUserAsync(string userId)
        {
            var genericResponse = new GenericResponse<bool>();
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                genericResponse.Message = "Invalid user to Reject ";

                return genericResponse;
            }
            user.IsAccepted = false;
            _unitOfWork.Repository<AppUser, string>().Update(user);
            var result = await _unitOfWork.CompleteAsync();
            if (result > 0)
            {
                genericResponse.StatusCode = StatusCodes.Status200OK;
                genericResponse.Message = "Success to Reject user";
                return genericResponse;
            }

            genericResponse.StatusCode = StatusCodes.Status200OK;
            genericResponse.Message = "Failed to Reject user";
            return genericResponse;
        }
    }
}
