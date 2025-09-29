using BudgetService.Models;

namespace BudgetService.Repositories
{
    public interface IBudgetRepository
    {
        Task<List<Budget>> GetAllAsync(int userId);
        Task<List<Budget>> GetActiveAsync(int userId);
        Task<Budget?> GetByIdAsync(int id, int userId);
        Task<List<Budget>> GetByCategoryAsync(int categoryId, int userId);
        Task<Budget> AddAsync(Budget budget);
        Task UpdateAsync(Budget budget);
        Task DeleteAsync(Budget budget);
        Task<bool> ExistsAsync(string name, int userId, int? excludeId = null);
        Task UpdateSpentAmountAsync(int budgetId, decimal spentAmount, int userId);
    }
}