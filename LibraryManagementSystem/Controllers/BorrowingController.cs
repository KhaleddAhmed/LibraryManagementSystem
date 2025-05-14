using System.Security.Claims;
using LibraryManagementSystem.Core.DTOs.UserBorrowings;
using LibraryManagementSystem.Core.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    public class BorrowingController : BaseApiController
    {
        private readonly ILibrarianService _librarianService;
        private readonly IUserBorrowService _userBorrowService;

        public BorrowingController(
            ILibrarianService librarianService,
            IUserBorrowService userBorrowService
        )
        {
            _librarianService = librarianService;
            _userBorrowService = userBorrowService;
        }

        [Authorize(Roles = "Librarian")]
        [HttpGet("GetAllBorrowRequestsFromMembers")]
        public async Task<ActionResult> GetRequests()
        {
            var result = await _librarianService.GetAllBorrowRequestsFromMembersAsync();
            return Ok(result);
        }

        [Authorize(Roles = "Librarian")]
        [HttpPut("ApproveMemberBorrow")]
        public async Task<ActionResult> Approve(string userId, string bookName)
        {
            var result = await _librarianService.ApproveRequestedBorrowFromMemberAsync(
                userId,
                bookName
            );
            return Ok(result);
        }

        [Authorize(Roles = "Librarian")]
        [HttpPut("RejectMemberBorrow")]
        public async Task<ActionResult> Reject(string userId, string bookName)
        {
            var result = await _librarianService.RejectRequestedBorrowFromMemberAsync(
                userId,
                bookName
            );
            return Ok(result);
        }

        [Authorize(Roles = "Librarian")]
        [HttpGet("GetAllReturnedBooks")]
        public async Task<ActionResult> GetAllReturn()
        {
            var result = await _librarianService.GetAllReturnedBooksAsync();
            return Ok(result);
        }

        [Authorize(Roles = "Librarian")]
        [HttpPut("ApproveOnReturnedBook")]
        public async Task<ActionResult> ApproveReteruned(string userId, string title)
        {
            var result = await _librarianService.ApproveReturnedBook(userId, title);
            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpPost("BorrowBook")]
        public async Task<ActionResult> CreateBorrow(CreateBorrowDto createBorrowDto)
        {
            createBorrowDto.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _userBorrowService.CreateBorrowAsync(createBorrowDto);
            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpPost("GetAllBorrowings")]
        public async Task<ActionResult> GetAlll()
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _userBorrowService.GetAllUserBorrowingAsync(UserId);
            return Ok(result);
        }
    }
}
