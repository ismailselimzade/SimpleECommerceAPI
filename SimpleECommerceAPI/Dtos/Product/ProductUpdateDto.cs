using System.ComponentModel.DataAnnotations;

namespace SimpleECommerceAPI.Dtos.Product
{
    public record ProductUpdateDto
    (
        [Required(AllowEmptyStrings = false)][MaxLength(100)] string Name,
        [Required(AllowEmptyStrings = false)][MaxLength(1000)] string Description,
        [Required] [Range(0.01, (double)decimal.MaxValue)] decimal Price,
        [Required] [Range(0, int.MaxValue)] int Stock,
        [Required(AllowEmptyStrings = false)][MaxLength(500)] string ImageUrl,
        Guid CategoryId
    );
}
