using System.Security.Claims;
using LibraryManagementSystem.Core.DTOs.Book;
using LibraryManagementSystem.Core.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    public class BookController : BaseApiController
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [Authorize(Roles = "Admin,Librarian")]
        [HttpPost("CreateBook")]
        public async Task<ActionResult> CreateBook(CreateBookDto createBookDto)
        {
            var userName = User.FindFirstValue(ClaimTypes.GivenName);
            var result = await _bookService.CreateBookAsync(createBookDto, userName);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,Librarian")]
        [HttpPut("UpdateBook")]
        public async Task<ActionResult> Update(UpdateBookDto updateBookDto)
        {
            var userName = User.FindFirstValue(ClaimTypes.GivenName);

            var result = await _bookService.UpdateBookAsync(updateBookDto, userName);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,Librarian")]
        [HttpDelete("DeleteBook")]
        public async Task<ActionResult> Delete(int bookId)
        {
            var result = await _bookService.DeleteBookAsync(bookId);
            return Ok(result);
        }

        [HttpGet("GetAllBooks")]
        public async Task<ActionResult> GetAllBooks(int? categoryId)
        {
            var result = await _bookService.GetAllBooksAsync(categoryId);
            return Ok(result);
        }

        [HttpGet("GetBookDetails")]
        public async Task<ActionResult> GetBooks(int bookId)
        {
            var result = await _bookService.GetBookAsync(bookId);
            return Ok(result);
        }
    }
}
