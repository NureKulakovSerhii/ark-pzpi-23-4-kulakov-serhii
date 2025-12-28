using Data.DateTrensferObjects;
using Domain.Abstractions;
using Domain.DateTrensferObjects;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Services;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("register-user")]
        public async Task<ActionResult<User>> RegisterAsync(RegisterUserDto user)
        {
            var register = await _userService.RegisterUserAsync(user);
            if(register is null)
            {
                return BadRequest("Ви вже зареєструвались");
            }
            return Ok(user);
        }
        [HttpPost("login-user")]
        public async Task<ActionResult<TokenResponseDto>> LoginAsync(LoginUserDto user)
        {
            var result = await _userService.LoginAsync(user);
            if(result is null)
            {
                return BadRequest(new
                {
                    message = "Невірна пошта або пароль",
                    error = "InvalidCredentials",
                });
            }
            return Ok(result);
        }

        
        [Authorize(Roles = "Moderator")]
        [HttpPost("promote-user{userId}")]
        public async Task<IActionResult> PromoteUserToModerator(Guid userId)
        {
            var updatedUser = await _userService.PromoteToModerator(userId);
            if(updatedUser is null)
            {
                return BadRequest("Не вдалося признаичти роль модератора");
            }
            return Ok(updatedUser);
        }
        [HttpPost("create-refresh-token")]
        public async Task<ActionResult<TokenResponseDto>>RefrshToken(RefreshTokenRequestDto request)
        {
            var result = await _userService.RefreshTokens(request);
            if(result is null || result.JwtToken == null || result.RefreshToken == null)
            {
                return Unauthorized("Невірний рефреш токен");
            }
            return Ok(result);
        }
        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync([FromBody] LogoutResponseDto logoutResponseDto)
        {
            var result = await _userService.LogoutAsync(logoutResponseDto.userId, logoutResponseDto.token);
            if (!result)
                return BadRequest("Invalid refresh token");
            return Ok(new { message = "Logout successfully"});
        }
    } 
}
