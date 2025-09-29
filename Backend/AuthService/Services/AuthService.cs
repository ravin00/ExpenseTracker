using AuthService.Dtos;
using AuthService.Models;
using AuthService.Repositories;
using BCrypt.Net;
using Microsoft.Extensions.Logging;

namespace AuthService.Services
{
    public class AuthService
    {
        private readonly UserRepository _userRepository;
        private readonly ILogger<AuthService> _logger;

        public AuthService(UserRepository userRepository, ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<User> RegisterAsync(RegisterUserDto dto)
        {
            _logger.LogInformation("Attempting to register user with email: {Email}", dto.Email);

            // Check if user already exists by email
            var existingUserByEmail = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUserByEmail != null)
            {
                _logger.LogWarning("Registration attempt with existing email: {Email}", dto.Email);
                throw new InvalidOperationException("User with this email already exists");
            }

            // Check if username already exists
            var existingUserByUsername = await _userRepository.GetByUsernameAsync(dto.Username);
            if (existingUserByUsername != null)
            {
                _logger.LogWarning("Registration attempt with existing username: {Username}", dto.Username);
                throw new InvalidOperationException("User with this username already exists");
            }

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email.ToLowerInvariant(),
                PasswordHash = HashPassword(dto.Password)
            };

            await _userRepository.AddUserAsync(user);
            _logger.LogInformation("User registered successfully with ID: {UserId}", user.Id);
            return user;
        }

        public async Task<User?> AuthenticateAsync(LoginUserDto dto)
        {
            _logger.LogInformation("Authentication attempt for email: {Email}", dto.Email);

            var user = await _userRepository.GetByEmailAsync(dto.Email.ToLowerInvariant());
            if (user == null)
            {
                _logger.LogWarning("Authentication failed - user not found: {Email}", dto.Email);
                return null;
            }

            if (!user.IsActive)
            {
                _logger.LogWarning("Authentication failed - user account disabled: {Email}", dto.Email);
                return null;
            }

            if (!VerifyPassword(dto.Password, user.PasswordHash))
            {
                _logger.LogWarning("Authentication failed - invalid password for user: {Email}", dto.Email);
                return null;
            }

            // Update last login time
            await _userRepository.UpdateLastLoginAsync(user.Id);

            _logger.LogInformation("User authenticated successfully: {Email}", dto.Email);
            return user;
        }

        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, 12);
        }

        private static bool VerifyPassword(string password, string hash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hash);
            }
            catch
            {
                return false;
            }
        }
    }
}
