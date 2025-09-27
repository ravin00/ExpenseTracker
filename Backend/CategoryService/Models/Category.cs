using System.ComponentModel.DataAnnotations;

namespace CategoryService.Models
{
    public class Category
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }   // Comes from JWT claims
        
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public required string Name { get; set; }
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        [StringLength(7)]  // For hex color codes like #FF5733
        public string? Color { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}