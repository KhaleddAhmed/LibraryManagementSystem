using System.Security.Claims;
using LibraryManagementSystem.Core.DTOs.Category;
using LibraryManagementSystem.Core.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    public class CategoryController : BaseApiController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Authorize(Roles = "Admin,Librarian")]
        [HttpPost("CreateCategory")]
        public async Task<ActionResult> Create(CreateCategoryDto createCategoryDto)
        {
            var userName = User.FindFirstValue(ClaimTypes.GivenName);
            var result = await _categoryService.CreateCategoryAsync(createCategoryDto, userName);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,Librarian")]
        [HttpPut("UpdateCategory")]
        public async Task<ActionResult> Update(UpdateCategoryDto updateCategoryDto)
        {
            var userName = User.FindFirstValue(ClaimTypes.GivenName);

            var result = await _categoryService.UpdateCategoryAsync(updateCategoryDto, userName);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,Librarian")]
        [HttpDelete("DeleteCategory")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            return Ok(result);
        }

        [HttpGet("GetAllCategories")]
        public async Task<ActionResult> GetAll()
        {
            var result = await _categoryService.GetAllCategoriesAsync();
            return Ok(result);
        }

        [HttpGet("GetCategoryDetails")]
        public async Task<ActionResult> Get(int id)
        {
            var result = await _categoryService.GetCategoryAsync(id);
            return Ok(result);
        }
    }
}
