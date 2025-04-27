using LibraryManagementSystem.Core.DTOs.Category;
using LibraryManagementSystem.Core.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : BaseApiController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("CreateCategory")]
        public async Task<ActionResult> Create(CreateCategoryDto createCategoryDto)
        {
            var result = await _categoryService.CreateCategoryAsync(createCategoryDto);
            return Ok(result);
        }

        [HttpPut("UpdateCategory")]
        public async Task<ActionResult> Update(UpdateCategoryDto updateCategoryDto)
        {
            var result = await _categoryService.UpdateCategoryAsync(updateCategoryDto);
            return Ok(result);
        }

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
