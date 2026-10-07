using System.ComponentModel.DataAnnotations;

namespace SimpleECommerceAPI.Dtos.Cart
{
    public record CartItemAddDto
    (
        [Required] Guid ProductId, 
        [Range(1, int.MaxValue)] int Quantity
    );
}
