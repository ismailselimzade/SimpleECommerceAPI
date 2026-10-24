using SimpleECommerceAPI.Models.Enums;

namespace SimpleECommerceAPI.Dtos.Order
{
    public record OrderResponseDto
    (
        Guid OrderId,
        Status Status,
        decimal TotalAmount,
        PaymentMethod PaymentMethod,
        DateTime CreatedAt,
        IEnumerable<OrderItemsResponseDto> Items
    );
}
