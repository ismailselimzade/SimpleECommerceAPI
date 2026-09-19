using SimpleECommerceAPI.Dtos.Product;

namespace SimpleECommerceAPI.Services
{
    public interface IProductService
    {
        Task<List<ProductResponseDto>> GetAllProductsAsync();
        Task<ProductResponseDto?> GetProductByIdAsync(Guid productId);
        Task<ProductResponseDto> CreateProductAsync(ProductCreateDto dto);
        Task<ProductResponseDto?> UpdateProductAsync(Guid id, ProductUpdateDto dto);
        Task<bool> UpdateStockAsync(Guid productId, ProductStockUpdateDto dto);
        Task<bool> DeleteProductAsync(Guid productId);
    }
}
