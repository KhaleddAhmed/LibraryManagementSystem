using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LibraryManagementSystem.Core;
using LibraryManagementSystem.Core.DTOs.Librarian;
using LibraryManagementSystem.Core.Entities.User;
using LibraryManagementSystem.Core.Responses;
using LibraryManagementSystem.Core.Service.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Service.Librarian
{
    public class LibrarianService : ILibrarianService
    {
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public LibrarianService(
            IUserService userService,
            UserManager<AppUser> userManager,
            IMapper mapper,
            IUnitOfWork unitOfWork
        )
        {
            _userService = userService;
            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse<bool>> CreateLibrarianAsync(
            CreateLibrarianDto createLibrarianDto
        )
        {
            var genericResponse = new GenericResponse<bool>();
            if (createLibrarianDto is null)
            {
                genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                genericResponse.Message = "Invalid Data";

                return genericResponse;
            }

            var userExist = await _userService.CheckEmailExistAsync(createLibrarianDto.Email);
            if (userExist)
            {
                genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                genericResponse.Message = "This Email is already exist";
                return genericResponse;
            }

            var mappedUser = _mapper.Map<AppUser>(createLibrarianDto);

            var result = await _userManager.CreateAsync(mappedUser, createLibrarianDto.Password);

            if (!result.Succeeded)
            {
                genericResponse.StatusCode = StatusCodes.Status200OK;
                genericResponse.Message = "Failed to Create Librarian";
                genericResponse.Data = false;

                return genericResponse;
            }

            genericResponse.StatusCode = StatusCodes.Status201Created;
            genericResponse.Message = "Success to Create Librarian";
            genericResponse.Data = true;

            return genericResponse;
        }

        public async Task<GenericResponse<bool>> DeleteLibrarianAsync(string id)
        {
            var genericResponse = new GenericResponse<bool>();

            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                genericResponse.StatusCode = StatusCodes.Status404NotFound;
                genericResponse.Message = "Invalid User To delete";

                return genericResponse;
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                genericResponse.StatusCode = StatusCodes.Status200OK;
                genericResponse.Message = "Failed to delete librarian";

                genericResponse.Data = false;

                return genericResponse;
            }

            genericResponse.StatusCode = StatusCodes.Status200OK;
            genericResponse.Message = "Success to delete Librarian";

            genericResponse.Data = true;

            return genericResponse;
        }

        public async Task<GenericResponse<ListOfLibrarianDto>> GetAllLibrarianAsync()
        {
            var genericResponse = new GenericResponse<ListOfLibrarianDto>();

            var librarians = await _unitOfWork.Repository<AppUser, string>().GetAllAsync();

            if (!librarians.Any())
            {
                genericResponse.StatusCode = StatusCodes.Status200OK;
                genericResponse.Message = "No Librarians to show";

                return genericResponse;
            }

            var librarianDtos = new List<GetAllLibrarianDto>();

            foreach (var librarian in librarians)
            {
                if (await _userManager.IsInRoleAsync(librarian, "Librarian"))
                {
                    librarianDtos.Add(_mapper.Map<GetAllLibrarianDto>(librarian));
                }
            }

            genericResponse.StatusCode = StatusCodes.Status200OK;
            genericResponse.Message = "Sucess to retreive all librarins";
            genericResponse.Data = new ListOfLibrarianDto
            {
                Count = librarianDtos.Count,
                GetAllLibrarianDtos = librarianDtos,
            };

            return genericResponse;
        }

        public Task<GenericResponse<GetLibrarianDto>> GetLibrarianAsync(string librarianId)
        {
            throw new NotImplementedException();
        }

        public Task<GenericResponse<bool>> UpdateLibrarianAsync(
            UpdateLibrarianDto updateLibrarianDto
        )
        {
            throw new NotImplementedException();
        }
    }
}
