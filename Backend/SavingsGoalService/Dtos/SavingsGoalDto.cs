using System.ComponentModel.DataAnnotations;
using SavingsGoalService.Models;

namespace SavingsGoalService.Dtos
{
    public class SavingsGoalDto
    {
        [Required(ErrorMessage = "Savings goal name is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters")]
        public required string Name { get; set; }
        
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
        
        [Required(ErrorMessage = "Target amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Target amount must be greater than 0")]
        public decimal TargetAmount { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "Current amount cannot be negative")]
        public decimal CurrentAmount { get; set; } = 0;
        
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        
        public DateTime? TargetDate { get; set; }
        
        public SavingsGoalStatus Status { get; set; } = SavingsGoalStatus.Active;
        
        public SavingsGoalPriority Priority { get; set; } = SavingsGoalPriority.Medium;
        
        [RegularExpression(@"^#[0-9A-Fa-f]{6}$", ErrorMessage = "Color must be a valid hex color code")]
        public string? Color { get; set; }
        
        public int? CategoryId { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "Monthly contribution target cannot be negative")]
        public decimal MonthlyContributionTarget { get; set; } = 0;
        
        public bool IsAutoContribution { get; set; } = false;
    }
    
    public class SavingsContributionDto
    {
        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }
        
        [StringLength(200, ErrorMessage = "Notes cannot exceed 200 characters")]
        public string? Notes { get; set; }
        
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
    
    public class SavingsWithdrawalDto
    {
        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }
        
        [Required(ErrorMessage = "Reason is required")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Reason must be between 1 and 200 characters")]
        public required string Reason { get; set; }
        
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}