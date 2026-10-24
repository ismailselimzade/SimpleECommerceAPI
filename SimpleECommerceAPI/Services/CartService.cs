using Microsoft.EntityFrameworkCore;
using SimpleECommerceAPI.Data;
using SimpleECommerceAPI.Dtos.Cart;
using SimpleECommerceAPI.Exceptions;
using SimpleECommerceAPI.Models;

namespace SimpleECommerceAPI.Services
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _db;
        public CartService(AppDbContext appDbContext)
        {
            _db = appDbContext;
        }
        public async Task<CartGetResponseDto> GetCartAsync(Guid userId)
        {
            CartGetResponseDto? response = await _db.Carts
                .Where(cart => cart.UserId == userId)
                .Include(cart => cart.CartItems)
                .ThenInclude(cartItem => cartItem.Product)
                .Select(cart => new CartGetResponseDto
                (
                    cart.Id,
                    cart.CreatedAt,
                    cart.CartItems.Select(cartItem => new CartItemResponseDto
                    (
                            cartItem.Id,
                            cartItem.ProductId,
                            cartItem.Product.Name,
                            cartItem.Quantity,
                            cartItem.Product.Price

                    ))
                )).FirstOrDefaultAsync();


            return response;
        }

        public async Task<CartItemResponseDto> AddCartItemAsync(Guid userId, CartItemAddDto cartItemAddDto)
        {
            var product = await _db.Products.FindAsync(cartItemAddDto.ProductId);
            
            if (product == null)
            {
                throw new NotFoundException("Product", cartItemAddDto.ProductId.ToString());
            }

            if (product.Stock < cartItemAddDto.Quantity)
            {
                return null;
            }

            Cart? cart = await _db.Carts
                .Where(cart => cart.UserId == userId)
                .Include(cart => cart.CartItems)
                .FirstOrDefaultAsync();

            if (cart == null)
            {
                Cart createdCart = new() { UserId = userId, CreatedAt = DateTime.UtcNow };
                

                await _db.Carts.AddAsync(createdCart);
                await _db.SaveChangesAsync();
                
                CartItem createdCartItem = new()
                {
                    CartId = createdCart.Id,
                    ProductId = cartItemAddDto.ProductId,
                    Quantity = cartItemAddDto.Quantity,
                };
                await _db.CartItems.AddAsync(createdCartItem);
                await _db.SaveChangesAsync();

                return new CartItemResponseDto
                    (
                        createdCart.Id,
                        createdCartItem.ProductId, 
                        product.Name,
                        createdCartItem.Quantity, 
                        product.Price
                    );
            }

            var cartItem = cart.CartItems.FirstOrDefault(cartItem => cartItem.ProductId == cartItemAddDto.ProductId);

            if (cartItem != null)
            {
                if (product.Stock < cartItemAddDto.Quantity)
                {
                    return null;
                }
                cartItem.Quantity += cartItemAddDto.Quantity;

                await _db.SaveChangesAsync();

                return new CartItemResponseDto
                (
                    cart.Id,
                    cartItem.ProductId,
                    product.Name,
                    cartItem.Quantity,
                    product.Price
                );
            }
           
            CartItem newCartItem = new()
                {
                    CartId = cart.Id,
                    ProductId = cartItemAddDto.ProductId,
                    Quantity = cartItemAddDto.Quantity
                };

                await _db.CartItems.AddAsync(newCartItem);
                await _db.SaveChangesAsync();


                return new CartItemResponseDto
                    (
                        cart.Id,
                        newCartItem.ProductId,
                        product.Name,
                        newCartItem.Quantity,
                        product.Price
                    );  
        }
        
        public async Task<CartItemResponseDto> UpdateCartItemAsync(Guid userId, Guid cartItemId, CartItemUpdateDto cartItemUpdateDto)
        {
            CartItem? cartItem = await _db.CartItems
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(cartItem => cartItem.Id == cartItemId && cartItem.Cart.UserId == userId);

            if (cartItem == null)
            {
                throw new NotFoundException("CartItem", cartItemId.ToString());
            }

            cartItem.Quantity = cartItemUpdateDto.Quantity;
            await _db.SaveChangesAsync();

            return new CartItemResponseDto(cartItem.CartId, cartItem.ProductId, cartItem.Product.Name, cartItem.Quantity, cartItem.Product.Price);
        }

        public async Task<bool> DeleteCartAsync(Guid userId, Guid cartId)
        {
            Cart? cart = await _db.Carts
                .FirstOrDefaultAsync(cart => cart.Id == cartId && cart.UserId == userId);

            if (cart != null)
            {
                _db.Carts.Remove(cart);
                await _db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteCartItemAsync(Guid userId, Guid cartItemId)
        {
            CartItem? cartItem = await _db.CartItems
                .FirstOrDefaultAsync(cartItem =>  cartItem.Id == cartItemId && cartItem.Cart.UserId == userId);

            if (cartItem != null)
            {
                _db.CartItems.Remove(cartItem);
                await _db.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
