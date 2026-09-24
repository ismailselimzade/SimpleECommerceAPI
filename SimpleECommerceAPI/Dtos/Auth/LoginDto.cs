using System.ComponentModel.DataAnnotations;

namespace SimpleECommerceAPI.Dtos.Auth
{
    public record LoginDto(
        [Required(AllowEmptyStrings = false)][MaxLength(100)] string UserName,
        [Required(AllowEmptyStrings = false)][MinLength(6)][MaxLength(100)] string Password
    );
}
