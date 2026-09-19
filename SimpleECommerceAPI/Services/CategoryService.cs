using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SimpleECommerceAPI.Data;
using SimpleECommerceAPI.Dtos.Category;
using SimpleECommerceAPI.Exceptions;
using SimpleECommerceAPI.Models;

namespace SimpleECommerceAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _db;
        public CategoryService(AppDbContext db)
        {
            _db = db;
        }


        public async Task<List<CategoryResponseDto>> GetAllCategoriesAsync()
        {
            List<CategoryResponseDto> allCategories = await _db.Categories
                .Select(category => new CategoryResponseDto(category.Id, category.Name))
                .ToListAsync();

            return allCategories;
        }

        public async Task<CategoryResponseDto?> GetCategoryByIdAsync(Guid id)
        {
            CategoryResponseDto? category = await _db.Categories
                .Where(c => c.Id == id)
                .Select(c => new CategoryResponseDto(c.Id, c.Name))
                .FirstOrDefaultAsync();

            return category;
        }

        public async Task<CategoryResponseDto> CreateCategoryAsync(CategoryDto dto)
        {
            if (await _db.Categories.AnyAsync(c => c.Name == dto.Name))
            {
                throw new DuplicateResourceException("Category", "name", dto.Name);
            }

            Category category = new() { Name = dto.Name };
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();

            return new CategoryResponseDto(category.Id, category.Name);
        }

        public async Task<CategoryResponseDto?> UpdateCategoryAsync(Guid id, CategoryDto dto)
        {
            var category = await _db.Categories.FindAsync(id);
            
            if (category != null)
            {
                if (await _db.Categories.AnyAsync(c => c.Name == dto.Name && c.Id != id))
                {
                    throw new DuplicateResourceException("Category", "name", dto.Name);
                }

                category.Name = dto.Name;
                await _db.SaveChangesAsync();

                return new CategoryResponseDto(category.Id, category.Name);
            }
            
            return null;
        }

        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
            var category = await _db.Categories.FindAsync(id);

            if (category != null)
            {
                if (await _db.Products.AnyAsync(p => p.CategoryId == id))
                {
                    throw new InUseException("Category", id.ToString());
                }

                _db.Categories.Remove(category);
                await _db.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
