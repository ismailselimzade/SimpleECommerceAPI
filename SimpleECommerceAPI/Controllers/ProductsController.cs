using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleECommerceAPI.Common;
using SimpleECommerceAPI.Dtos.Product;
using SimpleECommerceAPI.Exceptions;
using SimpleECommerceAPI.QueryParameters;
using SimpleECommerceAPI.Services;

namespace SimpleECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PagedResult<ProductResponseDto>>> GetAllProducts([FromQuery] ProductQueryParameters queryParameters)
        {
            return Ok(await _productService.GetAllProductsAsync(queryParameters));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponseDto>> GetProductById(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductResponseDto>> CreateProduct(ProductCreateDto dto)
        {
            try
            {
                var response = await _productService.CreateProductAsync(dto);
                return CreatedAtAction(nameof(GetProductById), new { id = response.Id}, response);
            }
            catch(NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductResponseDto>> UpdateProduct(Guid id, ProductUpdateDto dto)
        {
            try
            {
                var updated = await _productService.UpdateProductAsync(id, dto);
                return updated == null ? NotFound() : Ok(updated);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{productId}/stock")]
        public async Task<IActionResult> UpdateStock(Guid productId, ProductStockUpdateDto dto)
        {
            var result = await _productService.UpdateStockAsync(productId, dto);
            return result ? Ok(result) : NotFound();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(Guid productId)
        {
            var result = await _productService.DeleteProductAsync(productId);
            return result ? NoContent() : NotFound();
        }
    }
}
