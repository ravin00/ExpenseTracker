using System.ComponentModel.DataAnnotations;

namespace SavingsGoalService.Models
{
    public class SavingsGoal
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }   // From JWT claims
        
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public required string Name { get; set; }
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        [Range(0.01, double.MaxValue, ErrorMessage = "Target amount must be greater than 0")]
        public decimal TargetAmount { get; set; }
        
        public decimal CurrentAmount { get; set; } = 0;
        
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        
        public DateTime? TargetDate { get; set; }
        
        public SavingsGoalStatus Status { get; set; } = SavingsGoalStatus.Active;
        
        public SavingsGoalPriority Priority { get; set; } = SavingsGoalPriority.Medium;
        
        [StringLength(7)]  // For hex color codes like #FF5733
        public string? Color { get; set; }
        
        public int? CategoryId { get; set; }  // Optional link to category
        
        public decimal MonthlyContributionTarget { get; set; } = 0;
        
        public bool IsAutoContribution { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? CompletedAt { get; set; }
        
        // Calculated Properties
        public decimal ProgressPercentage => TargetAmount > 0 ? Math.Min(100, (CurrentAmount / TargetAmount) * 100) : 0;
        public decimal RemainingAmount => Math.Max(0, TargetAmount - CurrentAmount);
        public bool IsCompleted => CurrentAmount >= TargetAmount;
    }
    
    public enum SavingsGoalStatus
    {
        Active,
        Paused,
        Completed,
        Cancelled
    }
    
    public enum SavingsGoalPriority
    {
        Low,
        Medium,
        High,
        Critical
    }
}