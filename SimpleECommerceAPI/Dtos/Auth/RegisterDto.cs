using System.ComponentModel.DataAnnotations;

namespace SimpleECommerceAPI.Dtos.Auth
{
    public record RegisterDto
    (
        [Required(AllowEmptyStrings = false)][MaxLength(100)] string UserName,
        [Required(AllowEmptyStrings = false)][EmailAddress] string Email,
        [Required(AllowEmptyStrings = false)][MinLength(6)][MaxLength(100)] string Password
    );
}
