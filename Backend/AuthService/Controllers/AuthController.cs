using AuthService.Dtos;
using AuthService.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly Services.AuthService _authService;
        private readonly JwtHelper _jwtHelper;
        private readonly ILogger<AuthController> _logger;

        public AuthController(Services.AuthService authService, JwtHelper jwtHelper, ILogger<AuthController> logger)
        {
            _authService = authService;
            _jwtHelper = jwtHelper;
            _logger = logger;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="dto">User registration data</param>
        /// <returns>User information</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _authService.RegisterAsync(dto);
                _logger.LogInformation("User {UserId} registered successfully", user.Id);

                return CreatedAtAction(
                    nameof(GetUserProfile),
                    new { id = user.Id },
                    new
                    {
                        Id = user.Id,
                        Username = user.Username,
                        Email = user.Email,
                        Message = "User registered successfully"
                    });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Registration failed: {Message}", ex.Message);
                return Conflict(new { Message = ex.Message });
            }
        }

        /// <summary>
        /// Authenticate user and return JWT token
        /// </summary>
        /// <param name="dto">Login credentials</param>
        /// <returns>JWT token</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _authService.AuthenticateAsync(dto);
            if (user == null)
            {
                _logger.LogWarning("Failed login attempt for email: {Email}", dto.Email);
                return Unauthorized(new { Message = "Invalid email or password" });
            }

            var token = _jwtHelper.GenerateToken(user);
            _logger.LogInformation("User {UserId} logged in successfully", user.Id);

            return Ok(new
            {
                Token = token,
                User = new
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email
                },
                Message = "Login successful"
            });
        }

        /// <summary>
        /// Get user profile (placeholder for future use)
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>User profile</returns>
        [HttpGet("profile/{id}")]
        public IActionResult GetUserProfile(int id)
        {
            // Placeholder for future implementation
            return Ok(new { Message = "Profile endpoint - not implemented yet" });
        }

        /// <summary>
        /// Health check endpoint
        /// </summary>
        /// <returns>Service status</returns>
        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { Status = "Healthy", Service = "AuthService", Timestamp = DateTime.UtcNow });
        }
    }
}
