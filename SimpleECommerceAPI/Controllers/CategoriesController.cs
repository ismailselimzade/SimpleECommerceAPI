using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleECommerceAPI.Dtos.Category;
using SimpleECommerceAPI.Exceptions;
using SimpleECommerceAPI.Services;

namespace SimpleECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryResponseDto>>> GetAllCategories()
        {
            return Ok(await _categoryService.GetAllCategoriesAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryResponseDto>> GetCategoryById(Guid id)
        {
            CategoryResponseDto? result = await _categoryService.GetCategoryByIdAsync(id);

            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryResponseDto>> CreateCategory([FromBody] CategoryDto dto)
        {
            try
            {
                CategoryResponseDto created = await _categoryService.CreateCategoryAsync(dto);
                return CreatedAtAction(nameof(GetCategoryById), new { id = created.Id }, created);
            }
            catch (DuplicateResourceException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryResponseDto>> UpdateCategory(Guid id, [FromBody] CategoryDto dto)
        {
            try
            {
                var updated = await _categoryService.UpdateCategoryAsync(id, dto);
                return updated == null ? NotFound() : Ok(updated);
            }
            catch (DuplicateResourceException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            try
            {
                var result = await _categoryService.DeleteCategoryAsync(id);

                return result ? NoContent() : NotFound();
            }
            catch(CategoryInUseException ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}
