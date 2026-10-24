using SimpleECommerceAPI.Models;
using SimpleECommerceAPI.Models.Enums;

namespace SimpleECommerceAPI.Dtos.Order
{
    public record OrderItemsResponseDto
    (
        Guid Id, 
        Guid ProductId, 
        string ProductName, 
        int Quantity, 
        decimal UnitPrice
    );
}
