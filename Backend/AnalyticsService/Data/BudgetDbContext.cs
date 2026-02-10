using Microsoft.EntityFrameworkCore;

namespace AnalyticsService.Data
{
    // Read-only DbContext for querying BudgetService database
    public class BudgetDbContext : DbContext
    {
        public BudgetDbContext(DbContextOptions<BudgetDbContext> options) : base(options)
        {
        }

        public DbSet<BudgetReadModel> Budgets { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BudgetReadModel>(entity =>
            {
                entity.ToTable("Budgets");
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Amount).HasColumnType("decimal(18,2)");
                entity.Property(b => b.SpentAmount).HasColumnType("decimal(18,2)");
            });
        }
    }

    // Read-only model for Budget data
    public class BudgetReadModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public decimal Amount { get; set; }
        public decimal SpentAmount { get; set; }
        public string Period { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
