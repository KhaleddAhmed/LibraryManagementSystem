using LibraryManagementSystem.Core.DTOs.Librarian;
using LibraryManagementSystem.Core.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LibrarianController : BaseApiController
    {
        private readonly ILibrarianService _librarianService;

        public LibrarianController(ILibrarianService librarianService)
        {
            _librarianService = librarianService;
        }

        [HttpPost("CreateLibrarian")]
        public async Task<ActionResult> Create([FromBody] CreateLibrarianDto dto)
        {
            var result = await _librarianService.CreateLibrarianAsync(dto);
            return Ok(result);
        }

        [HttpGet("GetAllLibrarians")]
        public async Task<ActionResult> GetAll()
        {
            var result = await _librarianService.GetAllLibrarianAsync();
            return Ok(result);
        }

        [HttpGet("GetLibrarianDetails")]
        public async Task<ActionResult> Get(string id)
        {
            var result = await _librarianService.GetLibrarianAsync(id);
            return Ok(result);
        }

        [HttpDelete("DeleteLibrarian")]
        public async Task<ActionResult> Delete(string id)
        {
            var result = await _librarianService.DeleteLibrarianAsync(id);
            return Ok(result);
        }

        [HttpPut("UpdateLibrarian")]
        public async Task<ActionResult> Update([FromBody] UpdateLibrarianDto updateLibrarianDto)
        {
            var result = await _librarianService.UpdateLibrarianAsync(updateLibrarianDto);
            return Ok(result);
        }

        [HttpGet("GetAllRegisteredUsers")]
        public async Task<ActionResult> GetAllUsers()
        {
            var result = await _librarianService.GetAllUserAsync();
            return Ok(result);
        }
    }
}
