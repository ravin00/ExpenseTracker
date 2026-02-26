using SavingsGoalService.Dtos;
using SavingsGoalService.Models;

namespace SavingsGoalService.Services
{
    public interface ISavingsGoalService
    {
        Task<List<SavingsGoal>> GetSavingsGoalsAsync(int userId, SavingsGoalStatus? status = null);
        Task<SavingsGoal?> GetSavingsGoalAsync(int id, int userId);
        Task<List<SavingsGoal>> GetSavingsGoalsByCategoryAsync(int categoryId, int userId);
        Task<List<SavingsGoal>> GetSavingsGoalsByPriorityAsync(int userId, SavingsGoalPriority priority);
        Task<List<SavingsGoal>> GetDueSoonAsync(int userId, int daysFromNow = 30);
        Task<SavingsGoal> CreateSavingsGoalAsync(int userId, SavingsGoalDto dto);
        Task<SavingsGoal> UpdateSavingsGoalAsync(int id, int userId, SavingsGoalDto dto);
        Task<SavingsGoal> AddContributionAsync(int id, int userId, SavingsContributionDto dto);
        Task<SavingsGoal> WithdrawAsync(int id, int userId, SavingsWithdrawalDto dto);
        Task DeleteSavingsGoalAsync(int id, int userId);
        Task<object> GetSavingsStatisticsAsync(int userId);
    }
}