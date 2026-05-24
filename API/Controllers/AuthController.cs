using Application.Servces.AuthService;
using Application.Servces.AuthService.DTOs;
using Application.Servces.UserService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
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


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginInputDto input)
        {
            var response = await _authService.LoginAsync(input);
            return Ok(response);
        }

        [HttpGet("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromQuery] string refreshToken)
        {
            var newAccessToken = await _authService.RefreshTokenAsync(refreshToken);
            return Ok(newAccessToken);
        }

        [Authorize]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return Ok();
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordInputDto input)
        {
            await _authService.ChangePasswordAsync(input);
            return Ok();
        }

        [Authorize]
        [HttpPut("UpdateCurrentUserProfile")]
        public async Task<IActionResult> UpdateCurrentUserProfile([FromBody] UpdateUserDto input)
        {
            await _authService.UpdateCurrentUserProfile(input);
            return Ok();
        }
    }
}
