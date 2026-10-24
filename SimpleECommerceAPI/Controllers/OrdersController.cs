using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleECommerceAPI.Dtos.Order;
using SimpleECommerceAPI.Exceptions;
using SimpleECommerceAPI.Models.Enums;
using SimpleECommerceAPI.Services;
using System.Security.Claims;

namespace SimpleECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<OrderResponseDto>> CreateOrder(OrderCreateDto orderCreateDto)
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                return Ok(await _orderService.CreateOrderAsync(userId, orderCreateDto));
            }
            catch(NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<OrderResponseDto>> GetOrderByIdAsync(Guid orderId)
        {
            Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var response = await _orderService.GetOrderByIdAsync(userId, orderId);

            return response == null ? NotFound(): Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetAllOrders()
        {
            Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            return Ok(await _orderService.GetAllOrdersAsync(userId));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{orderId}/{status}")]
        public async Task<ActionResult<OrderResponseDto>> UpdateOrderStatus(Guid orderId, Status status)
        {
            try
            {
                var response = await _orderService.UpdateOrderStatusAsync(orderId, status);
                return response == null ? NotFound(): Ok(response);
            }
            catch(NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
