using System.ComponentModel.DataAnnotations;

namespace BudgetService.Models
{
    public class Budget
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "UserId must be greater than 0")]
        public int UserId { get; set; }   // From JWT claims

        [Required(ErrorMessage = "Budget name is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Budget name must be between 1 and 100 characters")]
        public required string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        public int? CategoryId { get; set; }  // Optional link to category

        [Required(ErrorMessage = "Budget period is required")]
        public BudgetPeriod Period { get; set; } = BudgetPeriod.Monthly;

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; } = DateTime.UtcNow;

        public DateTime? EndDate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Spent amount cannot be negative")]
        public decimal SpentAmount { get; set; } = 0; // Track spending against budget

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Validation methods
        public bool IsValidName()
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                   Name.Length >= 1 &&
                   Name.Length <= 100;
        }

        public bool IsValidDescription()
        {
            return Description == null || Description.Length <= 500;
        }

        public bool IsValidAmount()
        {
            return Amount > 0;
        }

        public bool IsValidSpentAmount()
        {
            return SpentAmount >= 0;
        }

        public bool IsValidUserId()
        {
            return UserId > 0;
        }

        public bool IsValidDateRange()
        {
            return !EndDate.HasValue || EndDate.Value > StartDate;
        }

        // Computed properties
        public string DisplayName => Name?.Trim() ?? string.Empty;

        public decimal ProgressPercentage => Amount > 0 ? Math.Min(100, (SpentAmount / Amount) * 100) : 0;

        public decimal RemainingAmount => Math.Max(0, Amount - SpentAmount);

        public bool IsExceeded => SpentAmount > Amount;

        public bool IsCompleted => IsExceeded || (EndDate.HasValue && DateTime.UtcNow > EndDate.Value);

        public decimal ExceededAmount => IsExceeded ? SpentAmount - Amount : 0;

        public int DaysRemaining => EndDate.HasValue ? Math.Max(0, (EndDate.Value - DateTime.UtcNow).Days) : 0;

        public int DaysOld => (DateTime.UtcNow - CreatedAt).Days;

        public bool IsRecentlyCreated => DaysOld <= 7;

        public bool IsRecentlyUpdated => (DateTime.UtcNow - UpdatedAt).TotalHours <= 24;

        public bool IsNearDeadline => EndDate.HasValue && DaysRemaining <= 7 && DaysRemaining > 0;

        public string StatusText => IsCompleted ? "Completed" : IsExceeded ? "Over Budget" : IsActive ? "Active" : "Inactive";

        public BudgetHealthStatus HealthStatus
        {
            get
            {
                if (IsExceeded) return BudgetHealthStatus.OverBudget;
                if (ProgressPercentage >= 90) return BudgetHealthStatus.Warning;
                if (ProgressPercentage >= 70) return BudgetHealthStatus.Caution;
                return BudgetHealthStatus.Healthy;
            }
        }
    }

    public enum BudgetPeriod
    {
        Daily,
        Weekly,
        Monthly,
        Quarterly,
        Yearly,
        Custom
    }

    public enum BudgetHealthStatus
    {
        Healthy,
        Caution,
        Warning,
        OverBudget
    }
}