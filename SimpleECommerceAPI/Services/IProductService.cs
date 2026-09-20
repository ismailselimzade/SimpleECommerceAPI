using SimpleECommerceAPI.Common;
using SimpleECommerceAPI.Dtos.Product;
using SimpleECommerceAPI.QueryParameters;

namespace SimpleECommerceAPI.Services
{
    public interface IProductService
    {
        Task<PagedResult<ProductResponseDto>> GetAllProductsAsync(ProductQueryParameters queryParameters);
        Task<ProductResponseDto?> GetProductByIdAsync(Guid productId);
        Task<ProductResponseDto> CreateProductAsync(ProductCreateDto dto);
        Task<ProductResponseDto?> UpdateProductAsync(Guid id, ProductUpdateDto dto);
        Task<bool> UpdateStockAsync(Guid productId, ProductStockUpdateDto dto);
        Task<bool> DeleteProductAsync(Guid productId);
    }
}
