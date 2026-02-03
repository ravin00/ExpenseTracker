using AuthService.Dtos;
using AuthService.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
// using System.ComponentModel.DataAnnotations;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(
        Services.AuthService authService,
        JwtHelper jwtHelper,
        ILogger<AuthController> logger) : ControllerBase
    {
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
                var user = await authService.RegisterAsync(dto);
                logger.LogInformation("User {UserId} registered successfully", user.Id);

                return CreatedAtAction(
                    nameof(GetProfile),
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
                logger.LogWarning("Registration failed: {Message}", ex.Message);
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

            var user = await authService.AuthenticateAsync(dto);
            if (user == null)
            {
                logger.LogWarning("Failed login attempt for email: {Email}", dto.Email);
                return Unauthorized(new { Message = "Invalid email or password" });
            }

            var token = jwtHelper.GenerateToken(user);
            logger.LogInformation("User {UserId} logged in successfully", user.Id);

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
        [Authorize]
        public async Task<IActionResult> GetProfile(int id)
        {
            try
            {
                var user = await authService.GetUserByIdAsync(id);

                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                return Ok(new
                {
                    id = user.Id,
                    username = user.Username,
                    email = user.Email,
                    createdAt = user.CreatedAt,
                    updatedAt = user.UpdatedAt
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving profile", error = ex.Message });
            }
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