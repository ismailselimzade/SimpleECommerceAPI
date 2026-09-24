using System.ComponentModel.DataAnnotations;

namespace SimpleECommerceAPI.Dtos.Category
{
    public record CategoryDto([Required(AllowEmptyStrings = false)][MaxLength(100)] string Name);
}
