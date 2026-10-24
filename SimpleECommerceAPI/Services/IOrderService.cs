using SimpleECommerceAPI.Dtos.Order;
using SimpleECommerceAPI.Models.Enums;

namespace SimpleECommerceAPI.Services
{
    public interface IOrderService
    {
        Task<OrderResponseDto> CreateOrderAsync(Guid userId, OrderCreateDto orderCreateDto);
        Task<OrderResponseDto> GetOrderByIdAsync(Guid userId, Guid orderId);
        Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync(Guid userId);
        Task<OrderResponseDto> UpdateOrderStatusAsync(Guid orderId, Status status);
    }
}
