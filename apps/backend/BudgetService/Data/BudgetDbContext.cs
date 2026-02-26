using BudgetService.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetService.Data
{
    public class BudgetDbContext : DbContext
    {
        public BudgetDbContext(DbContextOptions<BudgetDbContext> options) : base(options) { }

        public DbSet<Budget> Budgets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Budget>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.SpentAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Period).HasConversion<string>();
                
                // Configure DateTime properties for UTC
                entity.Property(e => e.StartDate).HasColumnType("timestamp with time zone");
                entity.Property(e => e.EndDate).HasColumnType("timestamp with time zone");
                entity.Property(e => e.CreatedAt).HasColumnType("timestamp with time zone");
                entity.Property(e => e.UpdatedAt).HasColumnType("timestamp with time zone");
                
                entity.HasIndex(e => new { e.UserId, e.Name }).IsUnique();
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => new { e.UserId, e.CategoryId });
            });
        }
    }
}