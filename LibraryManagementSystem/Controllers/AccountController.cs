using LibraryManagementSystem.Core.DTOs.User;
using LibraryManagementSystem.Core.Service.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult> RegisterUser([FromBody] RegisterDto registerDto)
        {
            var result = await _userService.RegisterAsync(registerDto);
            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _userService.LoginAsync(loginDto);
            return Ok(result);
        }
    }
}
