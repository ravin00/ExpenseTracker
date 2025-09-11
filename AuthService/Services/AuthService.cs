using AuthService.Dtos;
using AuthService.Models;
using AuthService.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace AuthService.Services
{
    public class AuthService(UserRepository userRepository)
    {
        public async Task<User> RegisterAsync(RegisterUserDto dto)
        {
            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = HashPassword(dto.Password)
            };

            await userRepository.AddUserAsync(user);
            return user;
        }

        public async Task<User?> AuthenticateAsync(LoginUserDto dto)
        {
            var user = await userRepository.GetByEmailAsync(dto.Email);
            if (user == null || !VerifyPassword(dto.Password, user.PasswordHash))
                return null;

            return user;
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(sha.ComputeHash(bytes));
        }

        private bool VerifyPassword(string password, string hash)
        {
            return HashPassword(password) == hash;
        }
    }
}
