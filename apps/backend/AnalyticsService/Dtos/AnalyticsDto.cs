using AnalyticsService.Models;
using System.ComponentModel.DataAnnotations;

namespace AnalyticsService.Dtos
{
    public class AnalyticsDto
    {
        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; } = DateTime.UtcNow.Date;

        [Range(0, double.MaxValue, ErrorMessage = "Total expenses cannot be negative")]
        public decimal TotalExpenses { get; set; } = 0;

        [Range(0, double.MaxValue, ErrorMessage = "Total income cannot be negative")]
        public decimal TotalIncome { get; set; } = 0;

        [Range(0, double.MaxValue, ErrorMessage = "Total savings cannot be negative")]
        public decimal TotalSavings { get; set; } = 0;

        [Range(0, double.MaxValue, ErrorMessage = "Budget utilization cannot be negative")]
        public decimal BudgetUtilization { get; set; } = 0;

        public int CategoryId { get; set; }

        [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters")]
        public string? CategoryName { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Category amount cannot be negative")]
        public decimal CategoryAmount { get; set; } = 0;

        public AnalyticsPeriod Period { get; set; } = AnalyticsPeriod.Daily;

        public bool IsValidDate()
        {
            return Date <= DateTime.UtcNow.Date;
        }

        public bool IsValidAmounts()
        {
            return TotalExpenses >= 0 && TotalIncome >= 0 && TotalSavings >= 0 && BudgetUtilization >= 0 && CategoryAmount >= 0;
        }
    }

    public class AnalyticsRequestDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public AnalyticsPeriod Period { get; set; } = AnalyticsPeriod.Monthly;
        public int? CategoryId { get; set; }
    }

    public class ExpenseByCategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public decimal Percentage { get; set; }
        public int TransactionCount { get; set; }
    }

    public class SpendingTrendDto
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public AnalyticsPeriod Period { get; set; }
    }

    public class BudgetAnalysisDto
    {
        public int BudgetId { get; set; }
        public string BudgetName { get; set; } = string.Empty;
        public decimal BudgetAmount { get; set; }
        public decimal SpentAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public decimal UtilizationPercentage { get; set; }
        public bool IsOverBudget { get; set; }
    }

    public class FinancialSummaryDto
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal TotalSavings { get; set; }
        public decimal NetWorth { get; set; }
        public decimal SavingsRate { get; set; }
        public string FinancialHealth { get; set; } = string.Empty;
        public List<ExpenseByCategoryDto> TopCategories { get; set; } = new();
        public List<SpendingTrendDto> SpendingTrends { get; set; } = new();
        public List<BudgetAnalysisDto> BudgetAnalyses { get; set; } = new();
    }

    public class DashboardDto
    {
        public FinancialSummaryDto Summary { get; set; } = new();
        public List<ExpenseByCategoryDto> CategoryBreakdown { get; set; } = new();
        public List<SpendingTrendDto> MonthlyTrends { get; set; } = new();
        public List<BudgetAnalysisDto> BudgetOverview { get; set; } = new();
        public decimal MonthlyAverage { get; set; }
        public decimal YearlyProjection { get; set; }
        public string RecommendationMessage { get; set; } = string.Empty;
    }
}