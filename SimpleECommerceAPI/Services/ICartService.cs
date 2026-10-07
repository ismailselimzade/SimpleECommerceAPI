using SimpleECommerceAPI.Dtos.Cart;

namespace SimpleECommerceAPI.Services
{
    public interface ICartService
    {
        Task<CartGetResponseDto> GetCartAsync(Guid userId);
        Task<CartItemResponseDto> AddCartItemAsync(Guid userId, CartItemAddDto cartItemAddDto);
        Task<CartItemResponseDto> UpdateCartItemAsync(Guid userId, Guid cartItemId, CartItemUpdateDto cartItemUpdateDto);
        Task<bool> DeleteCartItemAsync(Guid userId, Guid cartItemId);
        Task<bool> DeleteCartAsync(Guid userId, Guid cartId);
    }
}
