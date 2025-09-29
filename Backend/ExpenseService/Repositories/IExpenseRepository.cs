using ExpenseService.Models;

namespace ExpenseService.Repositories
{
    public interface IExpenseRepository
    {
        Task<List<Expense>> GetAllAsync(int userId);
        Task<Expense?> GetByIdAsync(int id, int userId);
        Task AddAsync(Expense expense);
        Task UpdateAsync(Expense expense);
        Task DeleteAsync(Expense expense);
    }
}