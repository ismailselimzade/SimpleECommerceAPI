using SimpleECommerceAPI.Dtos.Category;

namespace SimpleECommerceAPI.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryResponseDto>> GetAllCategoriesAsync();
        Task<CategoryResponseDto?> GetCategoryByIdAsync(Guid id);
        Task<CategoryResponseDto> CreateCategoryAsync(CategoryDto dto);
        Task<CategoryResponseDto?> UpdateCategoryAsync(Guid id, CategoryDto dto);
        Task<bool> DeleteCategoryAsync(Guid id);
    }
}
