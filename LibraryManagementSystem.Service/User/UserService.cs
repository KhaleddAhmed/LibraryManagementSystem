using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LibraryManagementSystem.Core;
using LibraryManagementSystem.Core.DTOs.User;
using LibraryManagementSystem.Core.Entities.User;
using LibraryManagementSystem.Core.Responses;
using LibraryManagementSystem.Core.Service.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagementSystem.Service.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;

        public UserService(
            UserManager<AppUser> userManager,
            IMapper mapper,
            ITokenService tokenService,
            SignInManager<AppUser> signInManager
        )
        {
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        public async Task<bool> CheckEmailExistAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }

        public async Task<GenericResponse<UserDto>> LoginAsync(LoginDto loginDto)
        {
            var genericResponse = new GenericResponse<UserDto>();

            if (loginDto is null)
            {
                genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                genericResponse.Message = "Invalid Login";

                return genericResponse;
            }
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null)
            {
                genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                genericResponse.Message = "User is not Exist";

                return genericResponse;
            }
            var result = await _signInManager.CheckPasswordSignInAsync(
                user,
                loginDto.Password,
                false
            );
            if (!result.Succeeded)
            {
                genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                genericResponse.Message = "InCorrect Password";

                return genericResponse;
            }
            UserDto userDto = new UserDto()
            {
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager),
            };

            genericResponse.StatusCode = StatusCodes.Status200OK;
            genericResponse.Message = "Login Succeeded";

            genericResponse.Data = userDto;

            return genericResponse;
        }

        public async Task<GenericResponse<UserDto>> RegisterAsync(RegisterDto registerDto)
        {
            GenericResponse<UserDto> response = new GenericResponse<UserDto>();
            if (registerDto is null)
            {
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Message = "Please Enter valid Data ";

                return response;
            }

            if (await CheckEmailExistAsync(registerDto.Email))
            {
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Message = "Email Already Exists";

                return response;
            }

            var mappedUser = _mapper.Map<AppUser>(registerDto);
            mappedUser.UserName = mappedUser.Email.Split("@")[0];

            var result = await _userManager.CreateAsync(mappedUser, registerDto.Password);

            if (result.Succeeded)
            {
                if (registerDto.IsLibrarian == true)
                    await _userManager.AddToRoleAsync(mappedUser, "Librarian");
                else
                    await _userManager.AddToRoleAsync(mappedUser, "User");
            }

            if (!result.Succeeded)
            {
                response.StatusCode = StatusCodes.Status200OK;
                response.Message = "Failed to Create User";

                return response;
            }

            var userDto = new UserDto()
            {
                Email = mappedUser.Email,
                Token = await _tokenService.CreateTokenAsync(mappedUser, _userManager),
            };

            response.StatusCode = StatusCodes.Status200OK;
            response.Message = "User Created Successfully";
            response.Data = userDto;

            return response;
        }
    }
}
