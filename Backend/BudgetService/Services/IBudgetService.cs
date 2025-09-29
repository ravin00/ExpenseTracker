using BudgetService.Dtos;
using BudgetService.Models;

namespace BudgetService.Services
{
    public interface IBudgetService
    {
        Task<List<Budget>> GetBudgetsAsync(int userId, bool activeOnly = false);
        Task<Budget?> GetBudgetAsync(int id, int userId);
        Task<List<Budget>> GetBudgetsByCategoryAsync(int categoryId, int userId);
        Task<Budget> CreateBudgetAsync(int userId, BudgetDto dto);
        Task<Budget> UpdateBudgetAsync(int id, int userId, BudgetDto dto);
        Task DeleteBudgetAsync(int id, int userId);
        Task UpdateSpentAmountAsync(int budgetId, decimal spentAmount, int userId);
        decimal GetBudgetProgress(Budget budget);
        decimal GetRemainingAmount(Budget budget);
        bool IsBudgetExceeded(Budget budget);
    }
}