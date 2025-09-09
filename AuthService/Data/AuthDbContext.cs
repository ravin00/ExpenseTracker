using AuthService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Data
{

        public class AuthDbContext(DbContextOptions<AuthDbContext> options) : DbContext(options)
        {
        public DbSet<User> Users { get; set; }
        }
    
}