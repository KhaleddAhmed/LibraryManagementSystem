using LibraryManagementSystem.Core.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseApiController
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPut("AcceptUser")]
        public async Task<ActionResult> Accept(string userId)
        {
            var result = await _adminService.AcceptUserAsync(userId);
            return Ok(result);
        }

        [HttpPut("RejectUser")]
        public async Task<ActionResult> Reject(string userId)
        {
            var result = await _adminService.RejectUserAsync(userId);
            return Ok(result);
        }

        [HttpGet("GetAllUserBorrowings")]
        public async Task<ActionResult> GetBorrowings()
        {
            var result = await _adminService.GetAllBorrowingsAdminAsync();
            return Ok(result);
        }
    }
}
