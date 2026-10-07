using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleECommerceAPI.Dtos.Cart;
using SimpleECommerceAPI.Exceptions;
using SimpleECommerceAPI.Models;
using SimpleECommerceAPI.Services;
using System.Security.Claims;

namespace SimpleECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<ActionResult<CartGetResponseDto>> GetCarts()
        {
            Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var response = await _cartService.GetCartAsync(userId);

            return response == null ? NotFound() : Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<CartItemResponseDto>> AddCartItemAsync(CartItemAddDto cartItemAddDto)
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                var response = await _cartService.AddCartItemAsync(userId, cartItemAddDto);

                if (response == null)
                {
                    return BadRequest("Insufficient stock.");
                }

                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{cartItemId}")]
        public async Task<ActionResult<CartItemResponseDto>> UpdateCartItemAsync(Guid cartItemId, CartItemUpdateDto cartItemUpdateDto)
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                var response = await _cartService.UpdateCartItemAsync(userId, cartItemId, cartItemUpdateDto);

                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("carts/{cartId}")]
        public async Task<IActionResult> DeleteCartAsync(Guid cartId)
        {
            Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            bool response = await _cartService.DeleteCartAsync(userId, cartId);

            return response == true ? NoContent(): NotFound();
        }

        [HttpDelete("cartItems/{cartItemId}")]
        public async Task<IActionResult> DeleteCartItemAsync(Guid cartItemId)
        {
            Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            bool response = await _cartService.DeleteCartItemAsync(userId, cartItemId);

            return response == true ? NoContent() : NotFound();
        }
    }
}
