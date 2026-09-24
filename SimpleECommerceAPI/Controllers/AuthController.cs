using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleECommerceAPI.Dtos.Auth;
using SimpleECommerceAPI.Exceptions;
using SimpleECommerceAPI.Services;
using System.Data;

namespace SimpleECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto dto)
        {
            try
            {
                return Ok(await _authService.RegisterAsync(dto));
            }
            catch (DuplicateResourceException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
        {
            var response = await _authService.LoginAsync(dto);

            return response == null ? Unauthorized(): Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResponseDto>> Refresh(RefreshTokenDto dto)
        {
            var response = await _authService.RefreshAsync(dto.Token);

            return response == null ? Unauthorized() : Ok(response);
        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout(RefreshTokenDto dto)
        {
            var result = await _authService.LogoutAsync(dto.Token);

            return result ? Ok(): Unauthorized();
        }
    }
}
