using Microsoft.EntityFrameworkCore;
using SimpleECommerceAPI.Data;
using SimpleECommerceAPI.Dtos.Product;
using SimpleECommerceAPI.Exceptions;
using SimpleECommerceAPI.Models;

namespace SimpleECommerceAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _db;
        public ProductService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<ProductResponseDto>> GetAllProductsAsync()
        {
            List<ProductResponseDto> products = await _db.Products
                .Include(p => p.Category)
                .Select(p => new ProductResponseDto
                (
                    p.Id, p.CategoryId, p.Name, p.Category.Name, p.Description,
                    p.Price, p.Stock, p.ImageUrl, p.CreatedAt
                ))
                .ToListAsync();

            return products;
        }

        public async Task<ProductResponseDto?> GetProductByIdAsync(Guid productId)
        {
            ProductResponseDto? product = await _db.Products
                .Include(p => p.Category)
                .Where(p => p.Id == productId)
                .Select(p => new ProductResponseDto(p.Id, p.CategoryId, p.Name, p.Category.Name, p.Description, p.Price, p.Stock, p.ImageUrl, p.CreatedAt))
                .FirstOrDefaultAsync();

            return product;
        }

        public async Task<ProductResponseDto> CreateProductAsync(ProductCreateDto dto)
        {
            Category? category = await _db.Categories
                .FirstOrDefaultAsync(c => c.Id == dto.CategoryId);

            if (category == null)
            {
                throw new NotFoundException("Category", dto.CategoryId.ToString());
            }

            Product product = new()
            {
                Name = dto.Name,
                CategoryId = dto.CategoryId,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                Price = dto.Price,
                Stock = dto.Stock,
                CreatedAt = DateTime.UtcNow
            };

            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();

            return new ProductResponseDto
                (
                    product.Id,
                    category.Id,
                    product.Name,
                    category.Name,
                    product.Description,
                    product.Price,
                    product.Stock,
                    product.ImageUrl,
                    product.CreatedAt
                );
        }

        public async Task<ProductResponseDto?> UpdateProductAsync(Guid id, ProductUpdateDto dto)
        {
            Product? product = await _db.Products.FindAsync(id);

            if (product != null)
            {
                Category? category = await _db.Categories.FindAsync(dto.CategoryId);

                if (category == null)
                {
                    throw new NotFoundException("Category", dto.CategoryId.ToString());
                }

                product.Name = dto.Name;
                product.Description = dto.Description;
                product.Price = dto.Price;
                product.Stock = dto.Stock;
                product.ImageUrl = dto.ImageUrl;
                product.CategoryId = dto.CategoryId;

                await _db.SaveChangesAsync();

                return new ProductResponseDto
                    (
                        product.Id,
                        product.CategoryId,
                        product.Name,
                        category.Name,
                        product.Description,
                        product.Price,
                        product.Stock,
                        product.ImageUrl,
                        product.CreatedAt
                    );
            }

            return null;
        }

        public async Task<bool> UpdateStockAsync(Guid productId, ProductStockUpdateDto dto)
        {
            Product? product = await _db.Products.FindAsync(productId);

            if (product == null)
            {
                return false;
            }

            product.Stock = dto.NewStock;
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteProductAsync(Guid productId)
        {
            Product? product = await _db.Products.FindAsync(productId);

            if (product == null)
            {
                return false;
            }

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
