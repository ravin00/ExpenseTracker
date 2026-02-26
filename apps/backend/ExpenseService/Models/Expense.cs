using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseService.Models
{
    [Table("Expenses")]
    public class Expense
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }   // Comes from JWT claims

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "Description must be between 1 and 500 characters")]
        public required string Description { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;

        [StringLength(100)]
        public string? Category { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;
    }
}