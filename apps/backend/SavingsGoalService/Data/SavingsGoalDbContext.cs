using SavingsGoalService.Models;
using Microsoft.EntityFrameworkCore;

namespace SavingsGoalService.Data
{
    public class SavingsGoalDbContext : DbContext
    {
        public SavingsGoalDbContext(DbContextOptions<SavingsGoalDbContext> options) : base(options) { }

        public DbSet<SavingsGoal> SavingsGoals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SavingsGoal>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Color).HasMaxLength(7);
                entity.Property(e => e.TargetAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.CurrentAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.MonthlyContributionTarget).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Status).HasConversion<string>();
                entity.Property(e => e.Priority).HasConversion<string>();
                
                // Indexes for performance
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => new { e.UserId, e.Status });
                entity.HasIndex(e => new { e.UserId, e.Name }).IsUnique();
                entity.HasIndex(e => new { e.UserId, e.CategoryId });
            });
        }
    }
}