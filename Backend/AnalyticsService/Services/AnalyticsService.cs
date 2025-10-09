using AnalyticsService.Dtos;
using AnalyticsService.Models;
using AnalyticsService.Repositories;

namespace AnalyticsService.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IAnalyticsRepository _repository;
        private readonly ILogger<AnalyticsService> _logger;

        public AnalyticsService(IAnalyticsRepository repository, ILogger<AnalyticsService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<Analytics>> GetAnalyticsAsync(int userId, AnalyticsPeriod? period = null)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid user ID", nameof(userId));

            return period.HasValue 
                ? await _repository.GetByPeriodAsync(userId, period.Value)
                : await _repository.GetAllAsync(userId);
        }

        public async Task<List<Analytics>> GetAnalyticsByDateRangeAsync(int userId, DateTime startDate, DateTime endDate)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid user ID", nameof(userId));

            if (startDate > endDate)
                throw new ArgumentException("Start date cannot be after end date");

            return await _repository.GetByDateRangeAsync(userId, startDate, endDate);
        }

        public async Task<Analytics?> GetAnalyticsAsync(int id, int userId)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid analytics ID", nameof(id));

            if (userId <= 0)
                throw new ArgumentException("Invalid user ID", nameof(userId));

            return await _repository.GetByIdAsync(id, userId);
        }

        public async Task<Analytics> CreateAnalyticsAsync(int userId, AnalyticsDto dto)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid user ID", nameof(userId));

            if (!dto.IsValidDate())
                throw new ArgumentException("Invalid date - cannot be in the future");

            if (!dto.IsValidAmounts())
                throw new ArgumentException("All amounts must be non-negative");

            // Check if analytics already exists for this date and period
            if (await _repository.ExistsAsync(userId, dto.Date, dto.Period))
                throw new InvalidOperationException($"Analytics already exists for {dto.Date:yyyy-MM-dd} with period {dto.Period}");

            var analytics = new Analytics
            {
                UserId = userId,
                Date = dto.Date.Date,
                TotalExpenses = dto.TotalExpenses,
                TotalIncome = dto.TotalIncome,
                TotalSavings = dto.TotalSavings,
                BudgetUtilization = dto.BudgetUtilization,
                CategoryId = dto.CategoryId,
                CategoryName = dto.CategoryName?.Trim(),
                CategoryAmount = dto.CategoryAmount,
                Period = dto.Period,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _logger.LogInformation($"Creating analytics for user {userId} for date {dto.Date:yyyy-MM-dd}");
            return await _repository.AddAsync(analytics);
        }

        public async Task<Analytics> UpdateAnalyticsAsync(int id, int userId, AnalyticsDto dto)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid analytics ID", nameof(id));

            if (userId <= 0)
                throw new ArgumentException("Invalid user ID", nameof(userId));

            var analytics = await _repository.GetByIdAsync(id, userId);
            if (analytics == null)
                throw new KeyNotFoundException($"Analytics with ID {id} not found");

            if (!dto.IsValidDate())
                throw new ArgumentException("Invalid date - cannot be in the future");

            if (!dto.IsValidAmounts())
                throw new ArgumentException("All amounts must be non-negative");

            // Check if another analytics exists for this date and period (excluding current one)
            if (await _repository.ExistsAsync(userId, dto.Date, dto.Period, id))
                throw new InvalidOperationException($"Another analytics record already exists for {dto.Date:yyyy-MM-dd} with period {dto.Period}");

            analytics.Date = dto.Date.Date;
            analytics.TotalExpenses = dto.TotalExpenses;
            analytics.TotalIncome = dto.TotalIncome;
            analytics.TotalSavings = dto.TotalSavings;
            analytics.BudgetUtilization = dto.BudgetUtilization;
            analytics.CategoryId = dto.CategoryId;
            analytics.CategoryName = dto.CategoryName?.Trim();
            analytics.CategoryAmount = dto.CategoryAmount;
            analytics.Period = dto.Period;

            _logger.LogInformation($"Updating analytics {id} for user {userId}");
            await _repository.UpdateAsync(analytics);
            return analytics;
        }

        public async Task DeleteAnalyticsAsync(int id, int userId)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid analytics ID", nameof(id));

            if (userId <= 0)
                throw new ArgumentException("Invalid user ID", nameof(userId));

            var analytics = await _repository.GetByIdAsync(id, userId);
            if (analytics == null)
                throw new KeyNotFoundException($"Analytics with ID {id} not found");

            _logger.LogInformation($"Deleting analytics {id} for user {userId}");
            await _repository.DeleteAsync(analytics);
        }

        public async Task<FinancialSummaryDto> GetFinancialSummaryAsync(int userId, DateTime startDate, DateTime endDate)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid user ID", nameof(userId));

            if (startDate > endDate)
                throw new ArgumentException("Start date cannot be after end date");

            var summary = await _repository.GetFinancialSummaryAsync(userId, startDate, endDate);
            
            return new FinancialSummaryDto
            {
                TotalIncome = summary.TotalIncome,
                TotalExpenses = summary.TotalExpenses,
                TotalSavings = summary.TotalSavings,
                NetWorth = summary.NetWorth,
                SavingsRate = summary.SavingsRate,
                FinancialHealth = summary.FinancialHealth,
                TopCategories = summary.TopCategories.Select(c => new ExpenseByCategoryDto
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    Amount = c.Amount,
                    Percentage = c.Percentage,
                    TransactionCount = c.TransactionCount
                }).ToList(),
                SpendingTrends = summary.SpendingTrends.Select(t => new SpendingTrendDto
                {
                    Date = t.Date,
                    Amount = t.Amount,
                    Period = t.Period
                }).ToList(),
                BudgetAnalyses = summary.BudgetAnalyses.Select(b => new BudgetAnalysisDto
                {
                    BudgetId = b.BudgetId,
                    BudgetName = b.BudgetName,
                    BudgetAmount = b.BudgetAmount,
                    SpentAmount = b.SpentAmount,
                    RemainingAmount = b.RemainingAmount,
                    UtilizationPercentage = b.UtilizationPercentage,
                    IsOverBudget = b.IsOverBudget
                }).ToList()
            };
        }

        public async Task<DashboardDto> GetDashboardDataAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid user ID", nameof(userId));

            var currentMonth = DateTime.UtcNow.Date;
            var startOfMonth = new DateTime(currentMonth.Year, currentMonth.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            var summary = await GetFinancialSummaryAsync(userId, startOfMonth, endOfMonth);
            var categoryBreakdown = await GetExpensesByCategoryAsync(userId, startOfMonth, endOfMonth);
            var monthlyTrends = await GetSpendingTrendsAsync(userId, AnalyticsPeriod.Monthly, 12);
            
            // Calculate monthly average from trends
            var monthlyAverage = monthlyTrends.Count > 0 ? monthlyTrends.Average(t => t.Amount) : 0;
            var yearlyProjection = monthlyAverage * 12;

            return new DashboardDto
            {
                Summary = summary,
                CategoryBreakdown = categoryBreakdown,
                MonthlyTrends = monthlyTrends,
                BudgetOverview = summary.BudgetAnalyses,
                MonthlyAverage = monthlyAverage,
                YearlyProjection = yearlyProjection,
                RecommendationMessage = GenerateRecommendation(summary)
            };
        }

        public async Task<List<ExpenseByCategoryDto>> GetExpensesByCategoryAsync(int userId, DateTime startDate, DateTime endDate)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid user ID", nameof(userId));

            var categories = await _repository.GetExpensesByCategoryAsync(userId, startDate, endDate);
            
            return categories.Select(c => new ExpenseByCategoryDto
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                Amount = c.Amount,
                Percentage = c.Percentage,
                TransactionCount = c.TransactionCount
            }).ToList();
        }

        public async Task<List<SpendingTrendDto>> GetSpendingTrendsAsync(int userId, AnalyticsPeriod period, int months = 12)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid user ID", nameof(userId));

            var trends = await _repository.GetSpendingTrendsAsync(userId, period, months);
            
            return trends.Select(t => new SpendingTrendDto
            {
                Date = t.Date,
                Amount = t.Amount,
                Period = t.Period
            }).ToList();
        }

        public async Task<Analytics> GenerateAnalyticsAsync(int userId, DateTime date, AnalyticsPeriod period)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid user ID", nameof(userId));

            // Check if analytics already exists
            if (await _repository.ExistsAsync(userId, date, period))
                throw new InvalidOperationException($"Analytics already exists for {date:yyyy-MM-dd} with period {period}");

            // This would typically integrate with other services to calculate actual values
            // For now, creating a basic analytics record
            var analytics = new Analytics
            {
                UserId = userId,
                Date = date.Date,
                Period = period,
                TotalExpenses = 0,
                TotalIncome = 0,
                TotalSavings = 0,
                BudgetUtilization = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _logger.LogInformation($"Generating analytics for user {userId} for date {date:yyyy-MM-dd}");
            return await _repository.AddAsync(analytics);
        }

        public async Task<bool> AnalyticsExistsAsync(int userId, DateTime date, AnalyticsPeriod period)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid user ID", nameof(userId));

            return await _repository.ExistsAsync(userId, date, period);
        }

        public decimal CalculateSavingsRate(decimal totalIncome, decimal totalSavings)
        {
            return totalIncome > 0 ? (totalSavings / totalIncome) * 100 : 0;
        }

        public string GetFinancialHealthStatus(decimal totalIncome, decimal totalExpenses, decimal savingsRate)
        {
            if (totalExpenses > totalIncome)
                return "Poor";
            
            if (savingsRate >= 20)
                return "Excellent";
            
            if (savingsRate >= 10)
                return "Good";
            
            return "Fair";
        }

        public string GenerateRecommendation(FinancialSummaryDto summary)
        {
            if (summary.TotalExpenses > summary.TotalIncome)
                return "Your expenses exceed your income. Consider reducing spending in high-cost categories.";
            
            if (summary.SavingsRate < 10)
                return "Try to increase your savings rate to at least 10% of your income for better financial health.";
            
            if (summary.SavingsRate >= 20)
                return "Excellent savings rate! Keep up the good financial habits.";
            
            return "Good financial management. Consider increasing your savings rate for even better financial security.";
        }
    }
}