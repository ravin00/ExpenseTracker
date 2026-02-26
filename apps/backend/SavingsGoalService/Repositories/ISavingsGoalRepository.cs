using SavingsGoalService.Models;

namespace SavingsGoalService.Repositories
{
    public interface ISavingsGoalRepository
    {
        Task<List<SavingsGoal>> GetAllAsync(int userId);
        Task<List<SavingsGoal>> GetByStatusAsync(int userId, SavingsGoalStatus status);
        Task<SavingsGoal?> GetByIdAsync(int id, int userId);
        Task<List<SavingsGoal>> GetByCategoryAsync(int categoryId, int userId);
        Task<List<SavingsGoal>> GetByPriorityAsync(int userId, SavingsGoalPriority priority);
        Task<List<SavingsGoal>> GetDueSoonAsync(int userId, int daysFromNow = 30);
        Task<SavingsGoal> AddAsync(SavingsGoal savingsGoal);
        Task UpdateAsync(SavingsGoal savingsGoal);
        Task DeleteAsync(SavingsGoal savingsGoal);
        Task<bool> ExistsAsync(string name, int userId, int? excludeId = null);
        Task<decimal> GetTotalSavingsAsync(int userId);
        Task<int> GetCompletedGoalsCountAsync(int userId);
    }
}