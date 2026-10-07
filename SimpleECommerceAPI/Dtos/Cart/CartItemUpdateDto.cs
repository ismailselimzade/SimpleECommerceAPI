using System.ComponentModel.DataAnnotations;

namespace SimpleECommerceAPI.Dtos.Cart
{
    public record CartItemUpdateDto([Range(1, int.MaxValue)] int Quantity);
}
