using SimpleECommerceAPI.Models;

namespace SimpleECommerceAPI.Dtos.Cart
{
    public record CartGetResponseDto(Guid Id, DateTime CreatedAt, IEnumerable<CartItemResponseDto> CartItems);
}
