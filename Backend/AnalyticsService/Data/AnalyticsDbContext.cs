using AnalyticsService.Models;
using Microsoft.EntityFrameworkCore;

namespace AnalyticsService.Data
{
    public class AnalyticsDbContext : DbContext
    {
        public AnalyticsDbContext(DbContextOptions<AnalyticsDbContext> options) : base(options) { }

        public DbSet<Analytics> Analytics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Analytics>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TotalExpenses).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TotalIncome).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TotalSavings).HasColumnType("decimal(18,2)");
                entity.Property(e => e.BudgetUtilization).HasColumnType("decimal(18,2)");
                entity.Property(e => e.CategoryAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Period).HasConversion<string>();
                entity.Property(e => e.CategoryName).HasMaxLength(100);
                
                // Indexes for better query performance
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => new { e.UserId, e.Date });
                entity.HasIndex(e => new { e.UserId, e.Period });
                entity.HasIndex(e => new { e.UserId, e.CategoryId });
                entity.HasIndex(e => new { e.UserId, e.Date, e.Period }).IsUnique();
            });
        }
    }
}