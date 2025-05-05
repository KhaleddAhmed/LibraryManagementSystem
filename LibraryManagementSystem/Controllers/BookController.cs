using LibraryManagementSystem.Core.DTOs.Book;
using LibraryManagementSystem.Core.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize(Roles = "Admin,Librarian")]
    public class BookController : BaseApiController
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost("CreateBook")]
        public async Task<ActionResult> CreateBook(CreateBookDto createBookDto)
        {
            var result = await _bookService.CreateBookAsync(createBookDto);
            return Ok(result);
        }

        [HttpPut("UpdateBook")]
        public async Task<ActionResult> Update(UpdateBookDto updateBookDto)
        {
            var result = await _bookService.UpdateBookAsync(updateBookDto);
            return Ok(result);
        }

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
