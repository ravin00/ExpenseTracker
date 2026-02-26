using AuthService.Data;
using AuthService.Dtos;
using AuthService.Models;
using AuthService.Repositories;
using AuthService.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace AuthService.Tests.Services
{
    public class AuthServiceTests : IDisposable
    {
        private readonly AuthDbContext _context;
        private readonly UserRepository _userRepository;
        private readonly Mock<ILogger<AuthService.Services.AuthService>> _mockLogger;
        private readonly Mock<ILogger<UserRepository>> _mockRepoLogger;
        private readonly AuthService.Services.AuthService _authService;

        public AuthServiceTests()
        {
            // Setup InMemory database
            var options = new DbContextOptionsBuilder<AuthDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AuthDbContext(options);
            _mockRepoLogger = new Mock<ILogger<UserRepository>>();
            _userRepository = new UserRepository(_context, _mockRepoLogger.Object);
            _mockLogger = new Mock<ILogger<AuthService.Services.AuthService>>();
            _authService = new AuthService.Services.AuthService(_userRepository, _mockLogger.Object);
        }

        [Fact]
        public async Task RegisterAsync_WithValidData_ShouldCreateUser()
        {
            // Arrange
            var registerDto = new RegisterUserDto
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "Password123!"
            };

            // Act
            var result = await _authService.RegisterAsync(registerDto);

            // Assert
            result.Should().NotBeNull();
            result.Username.Should().Be("testuser");
            result.Email.Should().Be("test@example.com");
            result.PasswordHash.Should().NotBeNullOrEmpty();

            // Verify password is hashed (not plain text)
            result.PasswordHash.Should().NotBe("Password123!");
        }

        [Fact]
        public async Task RegisterAsync_WithExistingEmail_ShouldThrowException()
        {
            // Arrange
            var existingUser = new User
            {
                Username = "existing",
                Email = "test@example.com",
                PasswordHash = "hashedpassword"
            };
            _context.Users.Add(existingUser);
            await _context.SaveChangesAsync();

            var registerDto = new RegisterUserDto
            {
                Username = "newuser",
                Email = "test@example.com",
                Password = "Password123!"
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _authService.RegisterAsync(registerDto));

            exception.Message.Should().Be("User with this email already exists");
        }

        [Fact]
        public async Task RegisterAsync_WithExistingUsername_ShouldThrowException()
        {
            // Arrange
            var existingUser = new User
            {
                Username = "testuser",
                Email = "existing@example.com",
                PasswordHash = "hashedpassword"
            };
            _context.Users.Add(existingUser);
            await _context.SaveChangesAsync();

            var registerDto = new RegisterUserDto
            {
                Username = "testuser",
                Email = "new@example.com",
                Password = "Password123!"
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _authService.RegisterAsync(registerDto));

            exception.Message.Should().Be("User with this username already exists");
        }

        [Fact]
        public async Task AuthenticateAsync_WithValidCredentials_ShouldReturnUser()
        {
            // Arrange
            var password = "Password123!";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, 12);

            var user = new User
            {
                Username = "testuser",
                Email = "test@example.com",
                PasswordHash = hashedPassword,
                IsActive = true
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var loginDto = new LoginUserDto
            {
                Email = "test@example.com",
                Password = password
            };

            // Act
            var result = await _authService.AuthenticateAsync(loginDto);

            // Assert
            result.Should().NotBeNull();
            result!.Email.Should().Be("test@example.com");
            result.Username.Should().Be("testuser");
        }

        [Fact]
        public async Task AuthenticateAsync_WithInvalidEmail_ShouldReturnNull()
        {
            // Arrange
            var loginDto = new LoginUserDto
            {
                Email = "nonexistent@example.com",
                Password = "Password123!"
            };

            // Act
            var result = await _authService.AuthenticateAsync(loginDto);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AuthenticateAsync_WithInvalidPassword_ShouldReturnNull()
        {
            // Arrange
            var user = new User
            {
                Username = "testuser",
                Email = "test@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("correctpassword", 12),
                IsActive = true
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var loginDto = new LoginUserDto
            {
                Email = "test@example.com",
                Password = "wrongpassword"
            };

            // Act
            var result = await _authService.AuthenticateAsync(loginDto);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AuthenticateAsync_WithInactiveUser_ShouldReturnNull()
        {
            // Arrange
            var password = "Password123!";
            var user = new User
            {
                Username = "testuser",
                Email = "test@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password, 12),
                IsActive = false // User is inactive
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var loginDto = new LoginUserDto
            {
                Email = "test@example.com",
                Password = password
            };

            // Act
            var result = await _authService.AuthenticateAsync(loginDto);

            // Assert
            result.Should().BeNull();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}