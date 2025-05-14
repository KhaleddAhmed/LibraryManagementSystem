using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Core;
using LibraryManagementSystem.Core.DTOs.Book;
using LibraryManagementSystem.Core.DTOs.UserBorrowings;
using LibraryManagementSystem.Core.Entities.Library;
using LibraryManagementSystem.Core.Responses;
using LibraryManagementSystem.Core.Service.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Service.Borrow
{
    public class UserBorrowService : IUserBorrowService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserBorrowService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse<bool>> CreateBorrowAsync(CreateBorrowDto createBorrowDto)
        {
            var genericResponse = new GenericResponse<bool>();

            var book = await _unitOfWork
                .Repository<Book, int>()
                .Get(B => B.Id == createBorrowDto.BookId)
                .Result.FirstOrDefaultAsync();

            if (book == null)
            {
                genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                genericResponse.Message = "Invalid Book To Borrow";

                return genericResponse;
            }

            if (createBorrowDto.BorrowDate.CompareTo(createBorrowDto.DueDate) > 0)
            {
                genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                genericResponse.Message = "You Cannot Borrow because Borrow date exceeds Due Date";

                return genericResponse;
            }

            if (book.IsAvaliable == false)
            {
                genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                genericResponse.Message = "Book is not avaliable to Borrow";

                return genericResponse;
            }

            var userBorrowing = new UserBorrowing()
            {
                AppUserId = createBorrowDto.UserId,
                BookId = createBorrowDto.BookId,
                BorrowDate = createBorrowDto.BorrowDate,
                DueDate = createBorrowDto.DueDate,
                AmountOfBorrow =
                    book.BorrowPricePerDay
                    * (createBorrowDto.DueDate.Day - createBorrowDto.BorrowDate.Day),
            };

            await _unitOfWork.Repository<UserBorrowing, int>().AddAsync(userBorrowing);
            var result = await _unitOfWork.CompleteAsync();
            if (result > 0)
            {
                book.IsAvaliable = false;
                _unitOfWork.Repository<Book, int>().Update(book);
                await _unitOfWork.CompleteAsync();
                genericResponse.StatusCode = StatusCodes.Status200OK;
                genericResponse.Message = "Success to Create UserBorrowing";
                genericResponse.Data = true;

                return genericResponse;
            }

            genericResponse.StatusCode = StatusCodes.Status200OK;
            genericResponse.Message = "Failed to Create UserBorrowing";
            genericResponse.Data = false;

            return genericResponse;
        }

        public async Task<GenericResponse<List<GetAllUserBorrowing>>> GetAllUserBorrowingAsync(
            string userId
        )
        {
            var genericResponse = new GenericResponse<List<GetAllUserBorrowing>>();
            var userBorrowings = await _unitOfWork
                .Repository<UserBorrowing, int>()
                .Get(Ub => Ub.AppUserId == userId)
                .Result.Include(uB => uB.Book)
                .ToListAsync();

            if (!userBorrowings.Any())
            {
                genericResponse.StatusCode = StatusCodes.Status200OK;
                genericResponse.Message = "No Borrowings to show";
                return genericResponse;
            }

            var listResult = new List<GetAllUserBorrowing>();

            foreach (var item in userBorrowings)
            {
                var resul = new GetAllUserBorrowing()
                {
                    BookTitle = item.Book.Title,
                    Amount = item.AmountOfBorrow,
                    RequestDate = item.RequestForBorrowDate,
                    DueDate = item.DueDate,
                    IsAccepted = item.IsBorrowApproved,
                };

                listResult.Add(resul);
            }

            genericResponse.StatusCode = StatusCodes.Status200OK;
            genericResponse.Message = "Success to retrive all userBorrowing";
            genericResponse.Data = listResult;
            return genericResponse;
        }
    }
}
