using ExpenseService.Data;
using ExpenseService.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseService.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly ExpenseDbContext _context;

        public ExpenseRepository(ExpenseDbContext context)
        {
            _context = context;
        }

        public async Task<List<Expense>> GetAllAsync(int userId)
        {
            return await _context.Expenses.Where(e => e.UserId == userId && e.IsActive).ToListAsync();
        }

        public async Task<Expense?> GetByIdAsync(int id, int userId)
        {
            return await _context.Expenses.FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId && e.IsActive);
        }

        public async Task AddAsync(Expense expense)
        {
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Expense expense)
        {
            expense.UpdatedAt = DateTime.UtcNow;
            _context.Expenses.Update(expense);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Expense expense)
        {
            expense.IsActive = false;
            expense.UpdatedAt = DateTime.UtcNow;
            _context.Expenses.Update(expense);
            await _context.SaveChangesAsync();
        }
    }
}
