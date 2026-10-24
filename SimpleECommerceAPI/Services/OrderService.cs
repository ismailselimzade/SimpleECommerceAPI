using Microsoft.EntityFrameworkCore;
using SimpleECommerceAPI.Data;
using SimpleECommerceAPI.Dtos.Order;
using SimpleECommerceAPI.Exceptions;
using SimpleECommerceAPI.Models;
using SimpleECommerceAPI.Models.Enums;

namespace SimpleECommerceAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _db;
        private readonly IPaymentService _paymentService;
        public OrderService(AppDbContext appDbContext, IPaymentService paymentService)
        {
            _db = appDbContext;
            _paymentService = paymentService;
        }

        public async Task<OrderResponseDto> CreateOrderAsync(Guid userId, OrderCreateDto orderCreateDto)
        {
            Cart? cart = await _db.Carts
                .Include(cart => cart.CartItems)
                .ThenInclude(cart => cart.Product)
                .FirstOrDefaultAsync(cart => cart.Id == orderCreateDto.CartId && cart.UserId == userId);

            if (cart == null)
            {
                throw new NotFoundException("Cart", orderCreateDto.CartId.ToString());
            }

            Order order = new()
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                Status = Status.Paid,
                PaymentMethod = orderCreateDto.PaymentMethod
            };


            foreach (var cartItem in cart.CartItems)
            {
                OrderItem orderItem = new()
                {
                    OrderId = order.Id,
                    ProductId = cartItem.ProductId,
                    ProductName = cartItem.Product.Name,
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.Product.Price
                };

                order.OrderItems.Add(orderItem);

                cartItem.Product.Stock -= cartItem.Quantity;
            }

            foreach (var orderItem in order.OrderItems)
            {
                order.TotalAmount += orderItem.UnitPrice * orderItem.Quantity;
            }

            await _paymentService.CreatePaymentAsync(order.TotalAmount, "usd");

            _db.Orders.Add(order);

            _db.Carts.Remove(cart);


            await _db.SaveChangesAsync();

            return new OrderResponseDto(order.Id, order.Status, order.TotalAmount, order.PaymentMethod, order.CreatedAt,
                order.OrderItems
                .Select(orderItem => new OrderItemsResponseDto
                (
                    orderItem.Id,
                    orderItem.ProductId,
                    orderItem.ProductName,
                    orderItem.Quantity,
                    orderItem.UnitPrice)
                ));
        }

        public async Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync(Guid userId)
        {
            IEnumerable<OrderResponseDto>? orders = await _db.Orders
                .Where(o => o.UserId == userId)
                .Select(o => new OrderResponseDto
                (
                        o.Id, o.Status, o.TotalAmount, o.PaymentMethod, o.CreatedAt,
                        o.OrderItems.Select(i => new OrderItemsResponseDto
                        (
                            i.Id, i.ProductId, i.ProductName, i.Quantity, i.UnitPrice
                        ))
                ))
                .ToListAsync();

            return orders;
        }

        public async Task<OrderResponseDto> GetOrderByIdAsync(Guid userId, Guid orderId)
        {
            OrderResponseDto? order = await _db.Orders
                .Where(o => o.Id == orderId && o.UserId == userId)
                .Select(o => new OrderResponseDto(o.Id, o.Status, o.TotalAmount, o.PaymentMethod, o.CreatedAt,
                o.OrderItems.Select(orderItems => new OrderItemsResponseDto
                (
                    orderItems.Id,
                    orderItems.ProductId,
                    orderItems.ProductName,
                    orderItems.Quantity,
                    orderItems.UnitPrice
                ))))
                .FirstOrDefaultAsync();

            return order;
        }

        public async Task<OrderResponseDto> UpdateOrderStatusAsync(Guid orderId, Status status)
        {
            Order? order = await _db.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.Id == orderId)
                .FirstOrDefaultAsync();

            if (order == null)
            {
                throw new NotFoundException("Order", orderId.ToString());
            }

            order.Status = status;

            await _db.SaveChangesAsync();

            return new OrderResponseDto(order.Id, order.Status, order.TotalAmount, order.PaymentMethod, order.CreatedAt,
                order.OrderItems.Select(orderItems => new OrderItemsResponseDto
                (
                    orderItems.Id,
                    orderItems.ProductId,
                    orderItems.ProductName,
                    orderItems.Quantity,
                    orderItems.UnitPrice
                )));
        }
    }
}
