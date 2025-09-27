using CategoryService.Data;
using CategoryService.Models;
using Microsoft.EntityFrameworkCore;

namespace CategoryService.Repositories
{
    public class CategoryRepository
    {
        private readonly CategoryDbContext _context;

        public CategoryRepository(CategoryDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllAsync(int userId)
        {
            return await _context.Categories
                .Where(c => c.UserId == userId)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id, int userId)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
        }

        public async Task<Category?> GetByNameAsync(string name, int userId)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == name && c.UserId == userId);
        }

        public async Task<Category> AddAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task UpdateAsync(Category category)
        {
            category.UpdatedAt = DateTime.UtcNow;
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(string name, int userId, int? excludeId = null)
        {
            return await _context.Categories
                .AnyAsync(c => c.Name == name && c.UserId == userId && (excludeId == null || c.Id != excludeId));
        }
    }
}