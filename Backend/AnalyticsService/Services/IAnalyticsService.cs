using AnalyticsService.Dtos;
using AnalyticsService.Models;

namespace AnalyticsService.Services
{
    public interface IAnalyticsService
    {
        Task<List<Analytics>> GetAnalyticsAsync(int userId, AnalyticsPeriod? period = null);
        Task<List<Analytics>> GetAnalyticsByDateRangeAsync(int userId, DateTime startDate, DateTime endDate);
        Task<Analytics?> GetAnalyticsAsync(int id, int userId);
        Task<Analytics> CreateAnalyticsAsync(int userId, AnalyticsDto dto);
        Task<Analytics> UpdateAnalyticsAsync(int id, int userId, AnalyticsDto dto);
        Task DeleteAnalyticsAsync(int id, int userId);
        Task<FinancialSummaryDto> GetFinancialSummaryAsync(int userId, DateTime startDate, DateTime endDate);
        Task<DashboardDto> GetDashboardDataAsync(int userId);
        Task<List<ExpenseByCategoryDto>> GetExpensesByCategoryAsync(int userId, DateTime startDate, DateTime endDate);
        Task<List<SpendingTrendDto>> GetSpendingTrendsAsync(int userId, AnalyticsPeriod period, int months = 12);
        Task<Analytics> GenerateAnalyticsAsync(int userId, DateTime date, AnalyticsPeriod period, int? categoryId = null);
        Task<bool> AnalyticsExistsAsync(int userId, DateTime date, AnalyticsPeriod period);
        decimal CalculateSavingsRate(decimal totalIncome, decimal totalSavings);
        string GetFinancialHealthStatus(decimal totalIncome, decimal totalExpenses, decimal savingsRate);
        string GenerateRecommendation(FinancialSummaryDto summary);
    }
}