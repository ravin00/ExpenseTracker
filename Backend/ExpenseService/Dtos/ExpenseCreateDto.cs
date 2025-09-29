using System.ComponentModel.DataAnnotations;

namespace ExpenseService.Dtos
{
    public class ExpenseCreateDto
    {
        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "Description must be between 1 and 500 characters")]
        public required string Description { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;

        [StringLength(100)]
        public string? Category { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }
    }
}