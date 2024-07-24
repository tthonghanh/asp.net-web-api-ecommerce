using ecommerce.Dtos.CategoryDtos;
using ecommerce.Interfaces;
using ecommerce.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepo;
        public CategoryController(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var categories = await _categoryRepo.GetAllCategoryAsync();
            var categoryDto = categories.Select(s => s.ToCategoryDto());

            return Ok(categoryDto);
        }

        [HttpGet("GetCategoryById/{id}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] string id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var category = await _categoryRepo.GetCategoryByIdAsync(id);
            if (category == null) return NotFound("Category not found");

            return Ok(category.ToCategoryDto());
        }

        [HttpPost("CreateCategory")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory([FromForm] CreateCategoryRequestDto categoryDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (categoryDto.ParentCategory != "0" && !await _categoryRepo.CategoryExist(categoryDto.ParentCategory)) return NotFound("Parent category not found");

            var categoryModel = categoryDto.ToCategoryFromCreate();
            await _categoryRepo.CreateCategoryAsync(categoryModel);

            return CreatedAtAction(nameof(GetCategoryById), new { id = categoryModel.Id }, categoryModel.ToCategoryDto());
        }

        [HttpPut("UpdateCategoryById/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategoryById([FromRoute] string id, [FromForm] UpdateCategoryRequestDto categoryDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var categoryModel = await _categoryRepo.UpdateCategoryAsync(id, categoryDto);
            if (categoryModel == null) return NotFound();

            return Ok(categoryModel.ToCategoryDto());
        }

        [HttpDelete("DeleteCategoryById/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategoryById([FromRoute] string id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var categoryModel = await _categoryRepo.DeleteCategoryAsync(id);
            if (categoryModel == null) return NotFound();

            return Ok("Delete successfully");
        }
    }
}