using AuthService.Data;
using AuthService.Models;
using AuthService.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace AuthService.Tests.Repositories
{
    public class UserRepositoryTests : IDisposable
    {
        private readonly AuthDbContext _context;
        private readonly Mock<ILogger<UserRepository>> _mockLogger;
        private readonly UserRepository _repository;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AuthDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AuthDbContext(options);
            _mockLogger = new Mock<ILogger<UserRepository>>();
            _repository = new UserRepository(_context, _mockLogger.Object);
        }

        [Fact]
        public async Task GetByEmailAsync_WithExistingEmail_ShouldReturnUser()
        {
            // Arrange
            var user = new User
            {
                Username = "testuser",
                Email = "test@example.com",
                PasswordHash = "hashedpassword"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByEmailAsync("test@example.com");

            // Assert
            result.Should().NotBeNull();
            result!.Email.Should().Be("test@example.com");
        }

        [Fact]
        public async Task GetByEmailAsync_WithNonExistingEmail_ShouldReturnNull()
        {
            // Act
            var result = await _repository.GetByEmailAsync("nonexistent@example.com");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByUsernameAsync_WithExistingUsername_ShouldReturnUser()
        {
            // Arrange
            var user = new User
            {
                Username = "testuser",
                Email = "test@example.com",
                PasswordHash = "hashedpassword"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByUsernameAsync("testuser");

            // Assert
            result.Should().NotBeNull();
            result!.Username.Should().Be("testuser");
        }

        [Fact]
        public async Task AddUserAsync_ShouldCreateUser()
        {
            // Arrange
            var user = new User
            {
                Username = "newuser",
                Email = "new@example.com",
                PasswordHash = "hashedpassword"
            };

            // Act
            var result = await _repository.AddUserAsync(user);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(0);

            var savedUser = await _context.Users.FindAsync(result.Id);
            savedUser.Should().NotBeNull();
            savedUser!.Username.Should().Be("newuser");
        }

        [Fact]
        public async Task UpdateLastLoginAsync_WithExistingUser_ShouldUpdateTimestamp()
        {
            // Arrange
            var user = new User
            {
                Username = "testuser",
                Email = "test@example.com",
                PasswordHash = "hashedpassword",
                LastLoginAt = null
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.UpdateLastLoginAsync(user.Id);

            // Assert
            result.Should().BeTrue();

            var updatedUser = await _context.Users.FindAsync(user.Id);
            updatedUser!.LastLoginAt.Should().NotBeNull();
            updatedUser.LastLoginAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public async Task ExistsAsync_WithExistingEmail_ShouldReturnTrue()
        {
            // Arrange
            var user = new User
            {
                Username = "testuser",
                Email = "test@example.com",
                PasswordHash = "hashedpassword"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.ExistsAsync("test@example.com");

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ExistsAsync_WithNonExistingEmail_ShouldReturnFalse()
        {
            // Act
            var result = await _repository.ExistsAsync("nonexistent@example.com");

            // Assert
            result.Should().BeFalse();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}