using AuthService.Data;
using AuthService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Repositories
{
    public class UserRepository
    {
        private readonly AuthDbContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(AuthDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            try
            {
                return await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user by email: {Email}", email);
                throw;
            }
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            try
            {
                return await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user by username: {Username}", username);
                throw;
            }
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Users.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user by ID: {UserId}", id);
                throw;
            }
        }

        public async Task<User> AddUserAsync(User user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("User created with ID: {UserId}", user.Id);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user with email: {Email}", user.Email);
                throw;
            }
        }

        public async Task<bool> UpdateLastLoginAsync(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user != null)
                {
                    user.LastLoginAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating last login for user: {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> ExistsAsync(string email, string? username = null)
        {
            try
            {
                var query = _context.Users.Where(u => u.Email == email.ToLowerInvariant());

                if (!string.IsNullOrEmpty(username))
                {
                    query = query.Where(u => u.Username == username);
                }

                return await query.AnyAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking user existence for email: {Email}", email);
                throw;
            }
        }
    }
}