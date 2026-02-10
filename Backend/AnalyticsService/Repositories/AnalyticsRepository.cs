using AnalyticsService.Data;
using AnalyticsService.Models;
using Microsoft.EntityFrameworkCore;

namespace AnalyticsService.Repositories
{
    public class AnalyticsRepository : IAnalyticsRepository
    {
        private readonly AnalyticsDbContext _context;
        private readonly ExpenseDbContext? _expenseContext;
        private readonly BudgetDbContext? _budgetContext;

        public AnalyticsRepository(
            AnalyticsDbContext context,
            ExpenseDbContext? expenseContext = null,
            BudgetDbContext? budgetContext = null)
        {
            _context = context;
            _expenseContext = expenseContext;
            _budgetContext = budgetContext;
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
            // Query the ExpenseService database directly for real expense data
            if (_expenseContext == null)
            {
                return new List<ExpenseByCategory>();
            }

            // Ensure dates are UTC for PostgreSQL
            var utcStart = DateTime.SpecifyKind(startDate, DateTimeKind.Utc);
            var utcEnd = DateTime.SpecifyKind(endDate, DateTimeKind.Utc);

            var expenses = await _expenseContext.Expenses
                .Where(e => e.UserId == userId && 
                           e.Date >= utcStart && 
                           e.Date <= utcEnd && 
                           e.IsActive &&
                           !string.IsNullOrEmpty(e.Category))
                .GroupBy(e => e.Category)
                .Select(g => new ExpenseByCategory
                {
                    CategoryId = 0, // We don't have category ID in expenses table
                    CategoryName = g.Key ?? "Unknown",
                    Amount = g.Sum(e => e.Amount),
                    TransactionCount = g.Count()
                })
                .OrderByDescending(e => e.Amount)
                .ToListAsync();

            // Calculate percentages
            var totalAmount = expenses.Sum(e => e.Amount);
            if (totalAmount > 0)
            {
                foreach (var category in expenses)
                {
                    category.Percentage = (category.Amount / totalAmount) * 100;
                }
            }

            return expenses;
        }

        public async Task<List<SpendingTrend>> GetSpendingTrendsAsync(int userId, AnalyticsPeriod period, int months = 12)
        {
            if (_expenseContext == null)
            {
                return new List<SpendingTrend>();
            }

            var startDate = DateTime.UtcNow.AddMonths(-months);

            var expenses = await _expenseContext.Expenses
                .Where(e => e.UserId == userId && e.Date >= startDate && e.IsActive)
                .Select(e => new { e.Date, e.Amount })
                .ToListAsync();

            // Group by period (daily, weekly, monthly, etc.)
            var trends = expenses
                .GroupBy(e => period switch
                {
                    AnalyticsPeriod.Daily => DateTime.SpecifyKind(e.Date.Date, DateTimeKind.Utc),
                    AnalyticsPeriod.Weekly => DateTime.SpecifyKind(e.Date.Date.AddDays(-(int)e.Date.DayOfWeek), DateTimeKind.Utc),
                    AnalyticsPeriod.Monthly => DateTime.SpecifyKind(new DateTime(e.Date.Year, e.Date.Month, 1), DateTimeKind.Utc),
                    AnalyticsPeriod.Quarterly => DateTime.SpecifyKind(new DateTime(e.Date.Year, ((e.Date.Month - 1) / 3) * 3 + 1, 1), DateTimeKind.Utc),
                    AnalyticsPeriod.Yearly => DateTime.SpecifyKind(new DateTime(e.Date.Year, 1, 1), DateTimeKind.Utc),
                    _ => DateTime.SpecifyKind(e.Date.Date, DateTimeKind.Utc)
                })
                .Select(g => new SpendingTrend
                {
                    Date = g.Key,
                    Amount = g.Sum(e => e.Amount),
                    Period = period
                })
                .OrderBy(s => s.Date)
                .ToList();

            return trends;
        }

        public async Task<decimal> GetTotalExpensesAsync(int userId, DateTime startDate, DateTime endDate)
        {
            if (_expenseContext == null)
            {
                return 0;
            }

            // Ensure dates are UTC for PostgreSQL
            var utcStart = DateTime.SpecifyKind(startDate, DateTimeKind.Utc);
            var utcEnd = DateTime.SpecifyKind(endDate, DateTimeKind.Utc);

            return await _expenseContext.Expenses
                .Where(e => e.UserId == userId && e.Date >= utcStart && e.Date <= utcEnd && e.IsActive)
                .SumAsync(e => e.Amount);
        }

        public async Task<decimal> GetTotalIncomeAsync(int userId, DateTime startDate, DateTime endDate)
        {
            // Income tracking would come from a separate IncomeService
            // For now, return 0 or estimate from analytics
            return 0;
        }

        public async Task<decimal> GetTotalSavingsAsync(int userId, DateTime startDate, DateTime endDate)
        {
            // Savings would come from SavingsGoalService contributions
            // For now, return 0
            return 0;
        }

        public async Task<FinancialSummary> GetFinancialSummaryAsync(int userId, DateTime startDate, DateTime endDate)
        {
            var totalIncome = await GetTotalIncomeAsync(userId, startDate, endDate);
            var totalExpenses = await GetTotalExpensesAsync(userId, startDate, endDate);
            var totalSavings = await GetTotalSavingsAsync(userId, startDate, endDate);
            var topCategories = await GetExpensesByCategoryAsync(userId, startDate, endDate);
            var spendingTrends = await GetSpendingTrendsAsync(userId, AnalyticsPeriod.Monthly, 6);

            // Get budget analyses from BudgetService database
            var budgetAnalyses = new List<BudgetAnalysis>();
            if (_budgetContext != null)
            {
                var budgets = await _budgetContext.Budgets
                    .Where(b => b.UserId == userId && 
                               b.IsActive && 
                               b.StartDate <= endDate && 
                               b.EndDate >= startDate)
                    .ToListAsync();

                budgetAnalyses = budgets.Select(b => new BudgetAnalysis
                {
                    BudgetId = b.Id,
                    BudgetName = $"{b.Period} Budget",
                    BudgetAmount = b.Amount,
                    SpentAmount = b.SpentAmount,
                    RemainingAmount = b.Amount - b.SpentAmount,
                    UtilizationPercentage = b.Amount > 0 ? (b.SpentAmount / b.Amount) * 100 : 0,
                    IsOverBudget = b.SpentAmount > b.Amount
                }).ToList();
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
                BudgetAnalyses = budgetAnalyses
            };
        }
    }
}