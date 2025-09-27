using CategoryService.Models;
using Microsoft.EntityFrameworkCore;

namespace CategoryService.Data
{
    public class CategoryDbContext : DbContext
    {
        public CategoryDbContext(DbContextOptions<CategoryDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Color).HasMaxLength(7);
                entity.HasIndex(e => new { e.UserId, e.Name }).IsUnique();
            });
        }
    }
}