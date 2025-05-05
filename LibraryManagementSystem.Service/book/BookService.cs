using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LibraryManagementSystem.Core;
using LibraryManagementSystem.Core.DTOs.Book;
using LibraryManagementSystem.Core.Entities.Library;
using LibraryManagementSystem.Core.Responses;
using LibraryManagementSystem.Core.Service.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Service.book
{
    public class BookService : IBookService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public BookService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse<bool>> CreateBookAsync(CreateBookDto createBookDto)
        {
            var genericResponse = new GenericResponse<bool>();
            if (createBookDto is null)
            {
                genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                genericResponse.Message = "Invalid Book Data";

                return genericResponse;
            }
            var BookNameExists = await _unitOfWork
                .Repository<Book, int>()
                .Get(B => B.Title == createBookDto.Title)
                .Result.FirstOrDefaultAsync();

            if (BookNameExists is not null)
            {
                genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                genericResponse.Message = "Book Name already Exists Cannot Create it Again";

                return genericResponse;
            }

            var mappedBook = _mapper.Map<Book>(createBookDto);

            await _unitOfWork.Repository<Book, int>().AddAsync(mappedBook);
            var result = await _unitOfWork.CompleteAsync();
            if (result > 0)
            {
                genericResponse.StatusCode = StatusCodes.Status200OK;
                genericResponse.Message = "Success to create Book";
                genericResponse.Data = true;
                return genericResponse;
            }

            genericResponse.StatusCode = StatusCodes.Status200OK;
            genericResponse.Message = "Failed to Create Book";
            return genericResponse;
        }

        public async Task<GenericResponse<bool>> DeleteBookAsync(int id)
        {
            var genericResponse = new GenericResponse<bool>();
            var bookToDelete = await _unitOfWork
                .Repository<Book, int>()
                .Get(B => B.Id == id)
                .Result.FirstOrDefaultAsync();
            if (bookToDelete is null)
            {
                genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                genericResponse.Message = "Invalid Book To delete";
                return genericResponse;
            }

            _unitOfWork.Repository<Book, int>().Delete(bookToDelete);
            var result = await _unitOfWork.CompleteAsync();
            if (result > 0)
            {
                genericResponse.StatusCode = StatusCodes.Status200OK;
                genericResponse.Message = "Success to delete Book";
                genericResponse.Data = true;
                return genericResponse;
            }

            genericResponse.StatusCode = StatusCodes.Status200OK;
            genericResponse.Message = "Failed to delete Book";
            return genericResponse;
        }

        public async Task<GenericResponse<GetBookDto>> GetBookAsync(int id)
        {
            var genericResponse = new GenericResponse<GetBookDto>();
            var book = await _unitOfWork
                .Repository<Book, int>()
                .Get(B => B.Id == id)
                .Result.Include(B => B.Category)
                .FirstOrDefaultAsync();
            if (book == null)
            {
                genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                genericResponse.Message = "Invalid Book to retrieve";

                return genericResponse;
            }

            var mappedBook = _mapper.Map<GetBookDto>(book);

            genericResponse.StatusCode = StatusCodes.Status200OK;
            genericResponse.Message = "Success to retrieve book details";

            genericResponse.Data = mappedBook;

            return genericResponse;
        }

        public async Task<GenericResponse<bool>> UpdateBookAsync(UpdateBookDto updateBookDto)
        {
            var genericResponse = new GenericResponse<bool>();
            if (updateBookDto is null)
            {
                genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                genericResponse.Message = "Invalid Data to update Book";

                return genericResponse;
            }

            var book = await _unitOfWork
                .Repository<Book, int>()
                .Get(B => B.Id == updateBookDto.Id)
                .Result.FirstOrDefaultAsync();

            if (book is null)
            {
                genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                genericResponse.Message = "Invalid Book Id To Update its details";

                return genericResponse;
            }

            _mapper.Map(updateBookDto, book);
            _unitOfWork.Repository<Book, int>().Update(book);
            var res = await _unitOfWork.CompleteAsync();
            if (res > 0)
            {
                genericResponse.StatusCode = StatusCodes.Status200OK;
                genericResponse.Message = "Success to Update Book ";
                genericResponse.Data = true;

                return genericResponse;
            }

            genericResponse.StatusCode = StatusCodes.Status200OK;
            genericResponse.Message = "Failed to update book";
            genericResponse.Data = false;

            return genericResponse;
        }

        public async Task<GenericResponse<List<GetAllBooksDto>>> GetAllBooksAsync(int? categoryId)
        {
            var genericResponse = new GenericResponse<List<GetAllBooksDto>>();
            if (categoryId.HasValue)
            {
                var allBooks = await _unitOfWork
                    .Repository<Book, int>()
                    .Get(B => B.CategoryId == categoryId)
                    .Result.ToListAsync();

                if (!allBooks.Any())
                {
                    genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                    genericResponse.Message = "No Books with this Category";

                    return genericResponse;
                }

                var mappedBooks = _mapper.Map<List<GetAllBooksDto>>(allBooks);

                genericResponse.StatusCode = StatusCodes.Status200OK;
                genericResponse.Message = "Success to get all books";

                genericResponse.Data = mappedBooks;

                return genericResponse;
            }

            var books = await _unitOfWork.Repository<Book, int>().GetAllAsync();
            if (!books.Any())
            {
                genericResponse.StatusCode = StatusCodes.Status200OK;
                genericResponse.Message = "No Books to show";
                return genericResponse;
            }

            var mappedBookList = _mapper.Map<List<GetAllBooksDto>>(books);
            genericResponse.StatusCode = StatusCodes.Status200OK;
            genericResponse.Message = "Success to retreuve all books";
            genericResponse.Data = mappedBookList;

            return genericResponse;
        }
    }
}
