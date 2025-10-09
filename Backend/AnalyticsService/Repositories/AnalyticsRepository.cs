using AnalyticsService.Data;
using AnalyticsService.Models;
using Microsoft.EntityFrameworkCore;

namespace AnalyticsService.Repositories
{
    public class AnalyticsRepository : IAnalyticsRepository
    {
        private readonly AnalyticsDbContext _context;

        public AnalyticsRepository(AnalyticsDbContext context)
        {
            _context = context;
        }

        public async Task<List<Analytics>> GetAllAsync(int userId)
        {
            return await _context.Analytics
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.Date)
                .ToListAsync();
        }

        public async Task<List<Analytics>> GetByPeriodAsync(int userId, AnalyticsPeriod period)
        {
            return await _context.Analytics
                .Where(a => a.UserId == userId && a.Period == period)
                .OrderByDescending(a => a.Date)
                .ToListAsync();
        }

        public async Task<List<Analytics>> GetByDateRangeAsync(int userId, DateTime startDate, DateTime endDate)
        {
            return await _context.Analytics
                .Where(a => a.UserId == userId && a.Date >= startDate && a.Date <= endDate)
                .OrderByDescending(a => a.Date)
                .ToListAsync();
        }

        public async Task<Analytics?> GetByIdAsync(int id, int userId)
        {
            return await _context.Analytics
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);
        }

        public async Task<Analytics?> GetByDateAndPeriodAsync(int userId, DateTime date, AnalyticsPeriod period)
        {
            return await _context.Analytics
                .FirstOrDefaultAsync(a => a.UserId == userId && a.Date.Date == date.Date && a.Period == period);
        }

        public async Task<Analytics> AddAsync(Analytics analytics)
        {
            _context.Analytics.Add(analytics);
            await _context.SaveChangesAsync();
            return analytics;
        }

        public async Task UpdateAsync(Analytics analytics)
        {
            analytics.UpdatedAt = DateTime.UtcNow;
            _context.Entry(analytics).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Analytics analytics)
        {
            _context.Analytics.Remove(analytics);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int userId, DateTime date, AnalyticsPeriod period, int? excludeId = null)
        {
            var query = _context.Analytics.Where(a => a.UserId == userId && a.Date.Date == date.Date && a.Period == period);
            
            if (excludeId.HasValue)
                query = query.Where(a => a.Id != excludeId.Value);
                
            return await query.AnyAsync();
        }

        public async Task<List<ExpenseByCategory>> GetExpensesByCategoryAsync(int userId, DateTime startDate, DateTime endDate)
        {
            return await _context.Analytics
                .Where(a => a.UserId == userId && a.Date >= startDate && a.Date <= endDate && a.CategoryId > 0)
                .GroupBy(a => new { a.CategoryId, a.CategoryName })
                .Select(g => new ExpenseByCategory
                {
                    CategoryId = g.Key.CategoryId,
                    CategoryName = g.Key.CategoryName ?? "Unknown",
                    Amount = g.Sum(a => a.CategoryAmount),
                    TransactionCount = g.Count()
                })
                .OrderByDescending(e => e.Amount)
                .ToListAsync();
        }

        public async Task<List<SpendingTrend>> GetSpendingTrendsAsync(int userId, AnalyticsPeriod period, int months = 12)
        {
            var startDate = DateTime.UtcNow.AddMonths(-months);
            
            return await _context.Analytics
                .Where(a => a.UserId == userId && a.Period == period && a.Date >= startDate)
                .GroupBy(a => a.Date.Date)
                .Select(g => new SpendingTrend
                {
                    Date = g.Key,
                    Amount = g.Sum(a => a.TotalExpenses),
                    Period = period
                })
                .OrderBy(s => s.Date)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalExpensesAsync(int userId, DateTime startDate, DateTime endDate)
        {
            return await _context.Analytics
                .Where(a => a.UserId == userId && a.Date >= startDate && a.Date <= endDate)
                .SumAsync(a => a.TotalExpenses);
        }

        public async Task<decimal> GetTotalIncomeAsync(int userId, DateTime startDate, DateTime endDate)
        {
            return await _context.Analytics
                .Where(a => a.UserId == userId && a.Date >= startDate && a.Date <= endDate)
                .SumAsync(a => a.TotalIncome);
        }

        public async Task<decimal> GetTotalSavingsAsync(int userId, DateTime startDate, DateTime endDate)
        {
            return await _context.Analytics
                .Where(a => a.UserId == userId && a.Date >= startDate && a.Date <= endDate)
                .SumAsync(a => a.TotalSavings);
        }

        public async Task<FinancialSummary> GetFinancialSummaryAsync(int userId, DateTime startDate, DateTime endDate)
        {
            var totalIncome = await GetTotalIncomeAsync(userId, startDate, endDate);
            var totalExpenses = await GetTotalExpensesAsync(userId, startDate, endDate);
            var totalSavings = await GetTotalSavingsAsync(userId, startDate, endDate);
            var topCategories = await GetExpensesByCategoryAsync(userId, startDate, endDate);
            var spendingTrends = await GetSpendingTrendsAsync(userId, AnalyticsPeriod.Monthly, 6);

            // Calculate percentages for categories
            if (topCategories.Count > 0 && totalExpenses > 0)
            {
                foreach (var category in topCategories)
                {
                    category.Percentage = (category.Amount / totalExpenses) * 100;
                }
            }

            var savingsRate = totalIncome > 0 ? (totalSavings / totalIncome) * 100 : 0;
            var financialHealth = totalExpenses > totalIncome ? "Poor" : 
                                 savingsRate >= 20 ? "Excellent" : 
                                 savingsRate >= 10 ? "Good" : "Fair";

            return new FinancialSummary
            {
                TotalIncome = totalIncome,
                TotalExpenses = totalExpenses,
                TotalSavings = totalSavings,
                NetWorth = totalIncome - totalExpenses,
                SavingsRate = savingsRate,
                FinancialHealth = financialHealth,
                TopCategories = topCategories.Take(5).ToList(),
                SpendingTrends = spendingTrends,
                BudgetAnalyses = new List<BudgetAnalysis>() // This would require integration with BudgetService
            };
        }
    }
}