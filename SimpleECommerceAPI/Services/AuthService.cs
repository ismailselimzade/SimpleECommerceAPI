using Microsoft.EntityFrameworkCore;
using SimpleECommerceAPI.Data;
using SimpleECommerceAPI.Dtos.Auth;
using SimpleECommerceAPI.Exceptions;
using SimpleECommerceAPI.Models;
using SimpleECommerceAPI.Models.Enums;

namespace SimpleECommerceAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly ITokenService _tokenService;
        private readonly IPasswordService _passwordService;

        public AuthService(AppDbContext db, ITokenService tokenService, IPasswordService passwordService)
        {
            _db = db;
            _tokenService = tokenService;
            _passwordService = passwordService;
        }
        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            if (await _db.Users.AnyAsync(u => u.Username == dto.UserName))
            {
                throw new DuplicateResourceException("User", "UserName", dto.UserName);
            }

            User user = new()
            {
                Username = dto.UserName,
                PasswordHash = _passwordService.HashPassword(dto.Password),
                Email = dto.Email,
                UserRole = UserRole.Customer,
                CreatedAt = DateTime.UtcNow
            };

            await _db.Users.AddAsync(user);

            string refreshToken = _tokenService.GenerateRefreshToken();
            string accessToken = _tokenService.GenerateAccessToken(user.Id, user.Username, user.UserRole);

            RefreshToken refresh = new()
            {
                User = user,
                TokenHash = _tokenService.HashToken(refreshToken),
                IsRevoked = false,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            await _db.RefreshTokens.AddAsync(refresh);

            await _db.SaveChangesAsync();

            return new AuthResponseDto(user.Username, refreshToken, accessToken);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _db.Users.Where(u => u.Username == dto.UserName).FirstOrDefaultAsync();

            if (user == null)
            {
                return null;
            }

            if (_passwordService.VerifyPassword(dto.Password, user.PasswordHash))
            {
                string accessToken = _tokenService.GenerateAccessToken(user.Id, user.Username, user.UserRole);
                string refreshToken = _tokenService.GenerateRefreshToken();

                RefreshToken refresh = new()
                {
                    User = user,
                    TokenHash = _tokenService.HashToken(refreshToken),
                    IsRevoked = false,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(7)
                };

                var oldTokens = await _db.RefreshTokens
                    .Where(rt => rt.UserId == user.Id && rt.IsRevoked == false)
                    .ToListAsync();

               foreach (var token in oldTokens)
                {
                    token.IsRevoked = true;
                }

                await _db.RefreshTokens.AddAsync(refresh);
                await _db.SaveChangesAsync();

                return new AuthResponseDto(user.Username, refreshToken, accessToken);
            }

            return null;
        }
        public async Task<AuthResponseDto> RefreshAsync(string token)
        {
            RefreshToken? refreshToken = await _db.RefreshTokens.Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.TokenHash == _tokenService.HashToken(token));

            if (refreshToken == null || refreshToken.IsRevoked || refreshToken.ExpiresAt < DateTime.UtcNow)
            {
                return null;
            }

            string newAccessToken = _tokenService.GenerateAccessToken(refreshToken.UserId, refreshToken.User.Username, refreshToken.User.UserRole);

            return new AuthResponseDto(refreshToken.User.Username, token, newAccessToken);
        }

        public async Task<bool> LogoutAsync(string token)
        {
            RefreshToken? refreshToken = await _db.RefreshTokens.
                FirstOrDefaultAsync(rt => rt.TokenHash == _tokenService.HashToken(token));

            if (refreshToken == null || refreshToken.IsRevoked || refreshToken.ExpiresAt < DateTime.UtcNow)
            {
                return false;
            }

            refreshToken.IsRevoked = true;
            await _db.SaveChangesAsync();
            return true;
        }


    }
}
