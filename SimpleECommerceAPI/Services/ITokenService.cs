using SimpleECommerceAPI.Models.Enums;

namespace SimpleECommerceAPI.Services
{
    public interface ITokenService
    {
        string GenerateRefreshToken();
        string GenerateAccessToken(Guid userId, string username, UserRole role);
        string HashToken(string token);
    }
}
