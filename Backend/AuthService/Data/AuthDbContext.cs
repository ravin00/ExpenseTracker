using AuthService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Data
{
        public class AuthDbContext : DbContext
        {
                public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
                {
                }

                public DbSet<User> Users { get; set; }

                protected override void OnModelCreating(ModelBuilder modelBuilder)
                {
                        base.OnModelCreating(modelBuilder);

                        // Configure User entity
                        modelBuilder.Entity<User>(entity =>
                        {
                                entity.HasKey(e => e.Id);

                                entity.HasIndex(e => e.Email)
                          .IsUnique()
                          .HasDatabaseName("IX_Users_Email");

                                entity.HasIndex(e => e.Username)
                          .IsUnique()
                          .HasDatabaseName("IX_Users_Username");

                                entity.Property(e => e.Email)
                          .IsRequired()
                          .HasMaxLength(100);

                                entity.Property(e => e.Username)
                          .IsRequired()
                          .HasMaxLength(50);

                                entity.Property(e => e.PasswordHash)
                          .IsRequired()
                          .HasMaxLength(255);

                                entity.Property(e => e.CreatedAt)
                          .HasDefaultValueSql("GETUTCDATE()");

                                entity.Property(e => e.UpdatedAt)
                          .HasDefaultValueSql("GETUTCDATE()");
                        });
                }

                public override int SaveChanges()
                {
                        UpdateTimestamps();
                        return base.SaveChanges();
                }

                public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
                {
                        UpdateTimestamps();
                        return base.SaveChangesAsync(cancellationToken);
                }

                private void UpdateTimestamps()
                {
                        var entries = ChangeTracker.Entries()
                            .Where(e => e.Entity is User && (
                                e.State == EntityState.Added ||
                                e.State == EntityState.Modified));

                        foreach (var entry in entries)
                        {
                                if (entry.Entity is User user)
                                {
                                        if (entry.State == EntityState.Added)
                                        {
                                                user.CreatedAt = DateTime.UtcNow;
                                        }
                                        user.UpdatedAt = DateTime.UtcNow;
                                }
                        }
                }
        }
}