using SimpleECommerceAPI.Dtos.Auth;

namespace SimpleECommerceAPI.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task<AuthResponseDto> RefreshAsync(string token);
        Task<bool> LogoutAsync(string token);
    }
}
