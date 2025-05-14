using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LibraryManagementSystem.Core;
using LibraryManagementSystem.Core.DTOs.Librarian;
using LibraryManagementSystem.Core.DTOs.User;
using LibraryManagementSystem.Core.DTOs.UserBorrowings;
using LibraryManagementSystem.Core.Entities.Library;
using LibraryManagementSystem.Core.Entities.User;
using LibraryManagementSystem.Core.Responses;
using LibraryManagementSystem.Core.Service.Contract;
using LibraryManagementSystem.Repository;
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

        public async Task<GenericResponse<bool>> ApproveRequestedBorrowFromMemberAsync(
            string memberId,
            string BookTitle
        )
        {
            var genericResponse = new GenericResponse<bool>();

            var userBorrowing = await _unitOfWork
                .Repository<UserBorrowing, int>()
                .Get(Ub => Ub.AppUserId == memberId)
                .Result.Include(Ub => Ub.Book)
                .Where(Ub => Ub.Book.Title == BookTitle)
                .FirstOrDefaultAsync();

            if (userBorrowing is null)
            {
                genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                genericResponse.Message = "Invalid User Borrowing to Approve";

                return genericResponse;
            }

            if (userBorrowing.Book.IsAvaliable == false)
            {
                genericResponse.StatusCode = StatusCodes.Status200OK;
                genericResponse.Message = "Book is not avaliable to Approve Borrow";

                return genericResponse;
            }

            userBorrowing.IsBorrowApproved = true;
            userBorrowing.Book.IsAvaliable = false;
            userBorrowing.BorrowStatus = BorrowStatus.Borrowed;
            userBorrowing.IsStillBorrowed = true;

            _unitOfWork.Repository<UserBorrowing, int>().Update(userBorrowing);
            var result = await _unitOfWork.CompleteAsync();
            if (result > 0)
            {
                genericResponse.StatusCode = StatusCodes.Status200OK;
                genericResponse.Message = "Success to approve member Borrow request";
                genericResponse.Data = true;
                return genericResponse;
            }
            genericResponse.StatusCode = StatusCodes.Status200OK;
            genericResponse.Message = "Success to Approve member Borrow request";
            genericResponse.Data = false;
            return genericResponse;
        }

        public async Task<GenericResponse<bool>> RejectRequestedBorrowFromMemberAsync(
            string memberId,
            string BookTitle
        )
        {
            var genericResponse = new GenericResponse<bool>();

            var userBorrowing = await _unitOfWork
                .Repository<UserBorrowing, int>()
                .Get(Ub => Ub.AppUserId == memberId)
                .Result.Include(Ub => Ub.Book)
                .Where(Ub => Ub.Book.Title == BookTitle)
                .FirstOrDefaultAsync();

            if (userBorrowing is null)
            {
                genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                genericResponse.Message = "Invalid User Borrowing to Reject";

                return genericResponse;
            }

            if (userBorrowing.Book.IsAvaliable == false)
            {
                genericResponse.StatusCode = StatusCodes.Status200OK;
                genericResponse.Message = "Book is not avaliable to Approve Borrow";

                return genericResponse;
            }

            userBorrowing.IsBorrowApproved = false;

            _unitOfWork.Repository<UserBorrowing, int>().Update(userBorrowing);
            var result = await _unitOfWork.CompleteAsync();
            if (result > 0)
            {
                genericResponse.StatusCode = StatusCodes.Status200OK;
                genericResponse.Message = "Success to reject member Borrow request";
                genericResponse.Data = true;
                return genericResponse;
            }
            genericResponse.StatusCode = StatusCodes.Status200OK;
            genericResponse.Message = "Success to Reject member Borrow request";
            genericResponse.Data = false;
            return genericResponse;
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
            mappedUser.UserName = mappedUser.Email.Split('@')[0];

            var result = await _userManager.CreateAsync(mappedUser, createLibrarianDto.Password);

            if (!result.Succeeded)
            {
                genericResponse.StatusCode = StatusCodes.Status200OK;
                genericResponse.Message = "Failed to Create Librarian";
                genericResponse.Data = false;

                return genericResponse;
            }

            await _userManager.AddToRoleAsync(mappedUser, "Librarian");

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

        public async Task<
            GenericResponse<List<GetAllBorrowRequestsFromMembersDto>>
        > GetAllBorrowRequestsFromMembersAsync()
        {
            var genericResponse = new GenericResponse<List<GetAllBorrowRequestsFromMembersDto>>();
            var userBorrowings = await _unitOfWork
                .Repository<UserBorrowing, int>()
                .GetAllAsyncAsQueryable()
                .Result.Include(Ub => Ub.AppUser)
                .Include(Ub => Ub.Book)
                .ThenInclude(Ub => Ub.Category)
                .ToListAsync();

            if (!userBorrowings.Any())
            {
                genericResponse.StatusCode = StatusCodes.Status200OK;
                genericResponse.Message = "No Requested Borrowings to show";

                return genericResponse;
            }

            var listResult = new List<GetAllBorrowRequestsFromMembersDto>();
            foreach (var borrow in userBorrowings)
            {
                var result = new GetAllBorrowRequestsFromMembersDto()
                {
                    BorrowerId = borrow.AppUserId,
                    NameBook = borrow.Book.Title,
                    NameMember = borrow.AppUser.FirstName,
                    Category = borrow.Book.Category.Name,
                };

                listResult.Add(result);
            }

            genericResponse.StatusCode = StatusCodes.Status200OK;
            genericResponse.Message = "Success to return all user Borrow requests";
            genericResponse.Data = listResult;

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

        public async Task<GenericResponse<GetLibrarianDto>> GetLibrarianAsync(string librarianId)
        {
            var genericResponse = new GenericResponse<GetLibrarianDto>();

            var librarian = await _userManager.FindByIdAsync(librarianId);
            if (librarian is null)
            {
                genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                genericResponse.Message = "Failed to Get Libririan";

                return genericResponse;
            }

            var mappedLibrarian = _mapper.Map<GetLibrarianDto>(librarian);

            genericResponse.StatusCode = StatusCodes.Status200OK;
            genericResponse.Message = "Sucess to retreive Librarian";
            genericResponse.Data = mappedLibrarian;

            return genericResponse;
        }

        public async Task<GenericResponse<bool>> UpdateLibrarianAsync(
            UpdateLibrarianDto updateLibrarianDto
        )
        {
            var genericResponse = new GenericResponse<bool>();
            if (updateLibrarianDto is null)
            {
                genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                genericResponse.Message = "Invalid Data";

                return genericResponse;
            }

            var librarian = await _userManager.FindByIdAsync(updateLibrarianDto.Id);

            if (librarian is null)
            {
                genericResponse.StatusCode = (int)StatusCodes.Status400BadRequest;
                genericResponse.Message = "Invalid Librarian Id to Update";

                return genericResponse;
            }

            _mapper.Map(updateLibrarianDto, librarian);
            _unitOfWork.Repository<AppUser, string>().Update(librarian);
            var resultOfUpdate = await _unitOfWork.CompleteAsync();

            if (resultOfUpdate > 0)
            {
                genericResponse.StatusCode = StatusCodes.Status200OK;
                genericResponse.Message = "Sucess to Update Librarian";

                genericResponse.Data = true;

                return genericResponse;
            }

            genericResponse.StatusCode = StatusCodes.Status200OK;
            genericResponse.Message = "Failed to Update Librarian";
            genericResponse.Data = false;

            return genericResponse;
        }

        public async Task<GenericResponse<List<GetAllReturnedBooksDto>>> GetAllReturnedBooksAsync()
        {
            var genericResponse = new GenericResponse<List<GetAllReturnedBooksDto>>();
            var userBorrowings = await _unitOfWork
                .Repository<UserBorrowing, int>()
                .GetAllAsyncAsQueryable()
                .Result.Include(Ub => Ub.AppUser)
                .Include(Ub => Ub.Book)
                .Where(Ub => Ub.UserWantsToReturn == true)
                .ToListAsync();

            if (!userBorrowings.Any())
            {
                genericResponse.StatusCode = StatusCodes.Status200OK;
                genericResponse.Message = "No User Retrued Books Requests";

                return genericResponse;
            }

            var listResult = new List<GetAllReturnedBooksDto>();
            foreach (var item in userBorrowings)
            {
                var result = new GetAllReturnedBooksDto()
                {
                    BookTitle = item.Book.Title,
                    BorrowDate = item.BorrowDate,
                    Borrower = item.AppUser.FirstName,
                    BorrowerId = item.AppUserId,
                };

                listResult.Add(result);
            }

            genericResponse.StatusCode = StatusCodes.Status200OK;
            genericResponse.Message = "Success to retrieve All Returned book requets";
            genericResponse.Data = listResult;
            return genericResponse;
        }

        public async Task<GenericResponse<bool>> ApproveReturnedBook(
            string borrowerId,
            string bookTitle
        )
        {
            var genericResponse = new GenericResponse<bool>();

            var userBorrowing = await _unitOfWork
                .Repository<UserBorrowing, int>()
                .Get(Ub => Ub.AppUserId == borrowerId)
                .Result.Include(Ub => Ub.Book)
                .Where(Ub => Ub.Book.Title == bookTitle)
                .FirstOrDefaultAsync();

            if (userBorrowing.ReturnedDate.Value.CompareTo(userBorrowing.DueDate) <= 0)
            {
                userBorrowing.IsReturnConfirmed = true;
                userBorrowing.Book.IsAvaliable = true;
                userBorrowing.BorrowStatus = BorrowStatus.Returned;
                userBorrowing.IsStillBorrowed = false;

                _unitOfWork.Repository<UserBorrowing, int>().Update(userBorrowing);
                var result = await _unitOfWork.CompleteAsync();
                if (result > 0)
                {
                    genericResponse.StatusCode = StatusCodes.Status200OK;
                    genericResponse.Message = "Success to approve return Book";
                    genericResponse.Data = true;

                    return genericResponse;
                }
                genericResponse.StatusCode = StatusCodes.Status200OK;
                genericResponse.Message = "Failed to approve return Book";
                genericResponse.Data = false;

                return genericResponse;
            }

            genericResponse.StatusCode = StatusCodes.Status200OK;
            genericResponse.Message = "You have to pay additional fees";
            genericResponse.Data = false;
            return genericResponse;
        }

        public async Task<GenericResponse<List<GetAllUserDto>>> GetAllUserAsync()
        {
            var genericResponse = new GenericResponse<List<GetAllUserDto>>();

            var users = await _unitOfWork.Repository<AppUser, string>().GetAllAsync();
            if (!users.Any())
            {
                genericResponse.StatusCode = StatusCodes.Status200OK;
                genericResponse.Message = "No Users to show";
                return genericResponse;
            }

            var listResult = new List<GetAllUserDto>();
            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "User"))
                {
                    var result = new GetAllUserDto() { Name = user.FirstName, Email = user.Email };
                    listResult.Add(result);
                }
            }

            genericResponse.StatusCode = StatusCodes.Status200OK;
            genericResponse.Message = "Sucess to return all users";
            genericResponse.Data = listResult;

            return genericResponse;
        }
    }
}
