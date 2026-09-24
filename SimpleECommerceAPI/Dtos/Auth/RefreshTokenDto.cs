using System.ComponentModel.DataAnnotations;

namespace SimpleECommerceAPI.Dtos.Auth
{
    public record RefreshTokenDto([Required(AllowEmptyStrings = false)] string Token);
}
