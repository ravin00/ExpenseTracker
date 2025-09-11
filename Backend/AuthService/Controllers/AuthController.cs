using AuthService.Dtos;
using AuthService.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(Services.AuthService authService, JwtHelper jwtHelper) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            var user = await authService.RegisterAsync(dto);
            return Ok(new { user.Id, user.Username, user.Email });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
        {
            var user = await authService.AuthenticateAsync(dto);
            if (user == null) return Unauthorized();

            var token = jwtHelper.GenerateToken(user);
            return Ok(new { Token = token });
        }
    }
}
