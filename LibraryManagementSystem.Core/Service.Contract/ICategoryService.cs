using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Core.DTOs.Category;
using LibraryManagementSystem.Core.Responses;

namespace LibraryManagementSystem.Core.Service.Contract
{
    public interface ICategoryService
    {
        Task<GenericResponse<bool>> CreateCategoryAsync(CreateCategoryDto dto, string userName);
        Task<GenericResponse<bool>> UpdateCategoryAsync(UpdateCategoryDto dto, string userName);
        Task<GenericResponse<bool>> DeleteCategoryAsync(int categoryId);

        Task<GenericResponse<GetCategoryDto>> GetCategoryAsync(int categoryId);
        Task<GenericResponse<List<GetAllCategoriesDto>>> GetAllCategoriesAsync();
    }
}
