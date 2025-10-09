using System.ComponentModel.DataAnnotations;

namespace AnalyticsService.Models
{
    public class Analytics
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "UserId must be greater than 0")]
        public int UserId { get; set; }

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

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Computed properties
        public decimal NetIncome => TotalIncome - TotalExpenses;

        public decimal SavingsRate => TotalIncome > 0 ? (TotalSavings / TotalIncome) * 100 : 0;

        public decimal ExpenseRatio => TotalIncome > 0 ? (TotalExpenses / TotalIncome) * 100 : 0;

        public bool IsPositiveCashFlow => NetIncome > 0;

        public bool IsHighSaver => SavingsRate >= 20; // 20% savings rate considered good

        public bool IsOverspending => TotalExpenses > TotalIncome;

        public string FinancialHealthStatus => IsOverspending ? "Poor" : IsHighSaver ? "Excellent" : SavingsRate >= 10 ? "Good" : "Fair";

        public decimal DailyAverageExpense => Period == AnalyticsPeriod.Monthly ? TotalExpenses / 30 : 
                                           Period == AnalyticsPeriod.Weekly ? TotalExpenses / 7 : TotalExpenses;

        public string PeriodDisplay => Period.ToString();

        public bool IsValidUserId() => UserId > 0;

        public bool IsValidDate() => Date <= DateTime.UtcNow.Date;

        public bool IsValidAmounts() => TotalExpenses >= 0 && TotalIncome >= 0 && TotalSavings >= 0;
    }

    public enum AnalyticsPeriod
    {
        Daily,
        Weekly,
        Monthly,
        Quarterly,
        Yearly
    }

    public class ExpenseByCategory
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public decimal Percentage { get; set; }
        public int TransactionCount { get; set; }
    }

    public class SpendingTrend
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public AnalyticsPeriod Period { get; set; }
    }

    public class BudgetAnalysis
    {
        public int BudgetId { get; set; }
        public string BudgetName { get; set; } = string.Empty;
        public decimal BudgetAmount { get; set; }
        public decimal SpentAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public decimal UtilizationPercentage { get; set; }
        public bool IsOverBudget { get; set; }
    }

    public class FinancialSummary
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal TotalSavings { get; set; }
        public decimal NetWorth { get; set; }
        public decimal SavingsRate { get; set; }
        public string FinancialHealth { get; set; } = string.Empty;
        public List<ExpenseByCategory> TopCategories { get; set; } = new();
        public List<SpendingTrend> SpendingTrends { get; set; } = new();
        public List<BudgetAnalysis> BudgetAnalyses { get; set; } = new();
    }
}