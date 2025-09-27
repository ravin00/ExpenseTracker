using System.ComponentModel.DataAnnotations;
using BudgetService.Models;

namespace BudgetService.Dtos
{
    public class BudgetDto
    {
        [Required(ErrorMessage = "Budget name is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Budget name must be between 1 and 100 characters")]
        public required string Name { get; set; }
        
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
        
        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }
        
        public int? CategoryId { get; set; }
        
        [Required(ErrorMessage = "Budget period is required")]
        public BudgetPeriod Period { get; set; }
        
        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        public bool IsActive { get; set; } = true;
    }
}