using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LibraryManagementSystem.Core;
using LibraryManagementSystem.Core.DTOs.Category;
using LibraryManagementSystem.Core.Entities.Library;
using LibraryManagementSystem.Core.Responses;
using LibraryManagementSystem.Core.Service.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Service.category
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GenericResponse<bool>> CreateCategoryAsync(
            CreateCategoryDto dto,
            string userName
        )
        {
            var genericResponse = new GenericResponse<bool>();
            if (dto is null)
            {
                genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                genericResponse.Message = "Invalid Data";

                return genericResponse;
            }

            var categoryNameExist = await _unitOfWork
                .Repository<Category, int>()
                .Get(C => C.Name == dto.Name)
                .Result.FirstOrDefaultAsync();

            if (categoryNameExist is not null)
            {
                genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                genericResponse.Message = "Category Name already exists";

                return genericResponse;
            }

            var mappedCategory = _mapper.Map<Category>(dto);
            mappedCategory.CreatedBy = userName;
            await _unitOfWork.Repository<Category, int>().AddAsync(mappedCategory);
            var result = await _unitOfWork.CompleteAsync();

            if (result > 0)
            {
                genericResponse.StatusCode = StatusCodes.Status200OK;

                genericResponse.Message = "Success to Create Category";
                genericResponse.Data = true;

                return genericResponse;
            }

            genericResponse.StatusCode = StatusCodes.Status200OK;

            genericResponse.Message = "Failed to Create Category";

            return genericResponse;
        }

        public async Task<GenericResponse<bool>> DeleteCategoryAsync(int categoryId)
        {
            var genericResponse = new GenericResponse<bool>();
            var category = await _unitOfWork.Repository<Category, int>().GetAsync(categoryId);
            if (category is null)
            {
                genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                genericResponse.Message = "Invalid Category to delete";

                return genericResponse;
            }

            _unitOfWork.Repository<Category, int>().Delete(category);
            var result = await _unitOfWork.CompleteAsync();
            if (result > 0)
            {
                genericResponse.StatusCode = StatusCodes.Status200OK;
                genericResponse.Message = "Success to delete Category";

                genericResponse.Data = true;
                return genericResponse;
            }

            genericResponse.StatusCode = StatusCodes.Status200OK;
            genericResponse.Message = "Failed to delete Category";
            return genericResponse;
        }

        public async Task<GenericResponse<List<GetAllCategoriesDto>>> GetAllCategoriesAsync()
        {
            var genericResponse = new GenericResponse<List<GetAllCategoriesDto>>();
            var categories = await _unitOfWork.Repository<Category, int>().GetAllAsync();
            if (!categories.Any())
            {
                genericResponse.StatusCode = StatusCodes.Status200OK;
                genericResponse.Message = "No Categories to show";

                return genericResponse;
            }

            var mappedCategories = _mapper.Map<List<GetAllCategoriesDto>>(categories);

            genericResponse.StatusCode = StatusCodes.Status200OK;
            genericResponse.Message = "Success to retrieve all categories";
            genericResponse.Data = mappedCategories;

            return genericResponse;
        }

        public async Task<GenericResponse<GetCategoryDto>> GetCategoryAsync(int categoryId)
        {
            var genericResponse = new GenericResponse<GetCategoryDto>();
            var category = await _unitOfWork.Repository<Category, int>().GetAsync(categoryId);
            if (category is null)
            {
                genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                genericResponse.Message = "Invalid Category to get details";

                return genericResponse;
            }

            var mappedCategory = _mapper.Map<GetCategoryDto>(category);

            genericResponse.StatusCode = StatusCodes.Status200OK;
            genericResponse.Message = "Success to get category Details";

            genericResponse.Data = mappedCategory;

            return genericResponse;
        }

        public async Task<GenericResponse<bool>> UpdateCategoryAsync(
            UpdateCategoryDto dto,
            string userName
        )
        {
            var genericResponse = new GenericResponse<bool>();
            var category = await _unitOfWork.Repository<Category, int>().GetAsync(dto.Id);
            if (category is null)
            {
                genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                genericResponse.Message = "Invalid Category to get details";

                return genericResponse;
            }

            _mapper.Map(dto, category);
            category.ModifiedAt = DateTime.Now;
            category.ModifiedBy = userName;
            _unitOfWork.Repository<Category, int>().Update(category);
            var result = await _unitOfWork.CompleteAsync();

            if (result > 0)
            {
                genericResponse.StatusCode = StatusCodes.Status200OK;
                genericResponse.Message = "Success to update category";
                genericResponse.Data = true;

                return genericResponse;
            }

            genericResponse.StatusCode = StatusCodes.Status200OK;
            genericResponse.Message = "Failed to update category";

            return genericResponse;
        }
    }
}
