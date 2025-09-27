using System.ComponentModel.DataAnnotations;

namespace BudgetService.Models
{
    public class Budget
    {
        public int Id { get; set; }

        public int UserId { get; set; }   // From JWT claims

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public required string Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Amount must be positive")]
        public decimal Amount { get; set; }

        public int? CategoryId { get; set; }  // Optional link to category

        public BudgetPeriod Period { get; set; } = BudgetPeriod.Monthly;

        public DateTime StartDate { get; set; } = DateTime.UtcNow;

        public DateTime? EndDate { get; set; }

        public decimal SpentAmount { get; set; } = 0; // Track spending against budget

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
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
}