using AnalyticsService.Models;

namespace AnalyticsService.Repositories
{
    public interface IAnalyticsRepository
    {
        Task<List<Analytics>> GetAllAsync(int userId);
        Task<List<Analytics>> GetByPeriodAsync(int userId, AnalyticsPeriod period);
        Task<List<Analytics>> GetByDateRangeAsync(int userId, DateTime startDate, DateTime endDate);
        Task<Analytics?> GetByIdAsync(int id, int userId);
        Task<Analytics?> GetByDateAndPeriodAsync(int userId, DateTime date, AnalyticsPeriod period);
        Task<Analytics> AddAsync(Analytics analytics);
        Task UpdateAsync(Analytics analytics);
        Task DeleteAsync(Analytics analytics);
        Task<bool> ExistsAsync(int userId, DateTime date, AnalyticsPeriod period, int? excludeId = null);
        Task<List<ExpenseByCategory>> GetExpensesByCategoryAsync(int userId, DateTime startDate, DateTime endDate);
        Task<List<SpendingTrend>> GetSpendingTrendsAsync(int userId, AnalyticsPeriod period, int months = 12);
        Task<decimal> GetTotalExpensesAsync(int userId, DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalIncomeAsync(int userId, DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalSavingsAsync(int userId, DateTime startDate, DateTime endDate);
        Task<FinancialSummary> GetFinancialSummaryAsync(int userId, DateTime startDate, DateTime endDate);
    }
}