using BudgetService.Data;
using BudgetService.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetService.Repositories
{
    public class BudgetRepository
    {
        private readonly BudgetDbContext _context;

        public BudgetRepository(BudgetDbContext context)
        {
            _context = context;
        }

        public async Task<List<Budget>> GetAllAsync(int userId)
        {
            return await _context.Budgets
                .Where(b => b.UserId == userId)
                .OrderBy(b => b.Name)
                .ToListAsync();
        }

        public async Task<List<Budget>> GetActiveAsync(int userId)
        {
            return await _context.Budgets
                .Where(b => b.UserId == userId && b.IsActive)
                .OrderBy(b => b.Name)
                .ToListAsync();
        }

        public async Task<Budget?> GetByIdAsync(int id, int userId)
        {
            return await _context.Budgets
                .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);
        }

        public async Task<List<Budget>> GetByCategoryAsync(int categoryId, int userId)
        {
            return await _context.Budgets
                .Where(b => b.CategoryId == categoryId && b.UserId == userId)
                .OrderBy(b => b.Name)
                .ToListAsync();
        }

        public async Task<Budget> AddAsync(Budget budget)
        {
            _context.Budgets.Add(budget);
            await _context.SaveChangesAsync();
            return budget;
        }

        public async Task UpdateAsync(Budget budget)
        {
            budget.UpdatedAt = DateTime.UtcNow;
            _context.Budgets.Update(budget);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Budget budget)
        {
            _context.Budgets.Remove(budget);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(string name, int userId, int? excludeId = null)
        {
            return await _context.Budgets
                .AnyAsync(b => b.Name == name && b.UserId == userId && (excludeId == null || b.Id != excludeId));
        }

        public async Task UpdateSpentAmountAsync(int budgetId, decimal spentAmount)
        {
            var budget = await _context.Budgets.FindAsync(budgetId);
            if (budget != null)
            {
                budget.SpentAmount = spentAmount;
                budget.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}