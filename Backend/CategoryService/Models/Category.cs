using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CategoryService.Models
{
    public class Category
    {
        public int Id { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "UserId must be greater than 0")]
        public int UserId { get; set; }   // Comes from JWT claims
        
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Category name must be between 1 and 100 characters")]
        public required string Name { get; set; }
        
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
        
        [StringLength(7, ErrorMessage = "Color must be exactly 7 characters for hex codes")]
        public string? Color { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Validation methods
        public bool IsValidColorFormat()
        {
            if (string.IsNullOrEmpty(Color)) return true; // Nullable field
            return Regex.IsMatch(Color, @"^#[0-9A-Fa-f]{6}$");
        }
        
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
        
        public bool IsValidUserId()
        {
            return UserId > 0;
        }
        
        // Computed properties
        public string DisplayName => Name?.Trim() ?? string.Empty;
        
        public string SafeColor => IsValidColorFormat() && !string.IsNullOrEmpty(Color) ? Color : "#6B7280"; // Default gray color
        
        public int DaysOld => (DateTime.UtcNow - CreatedAt).Days;
        
        public bool IsRecentlyCreated => DaysOld <= 7;
        
        public bool IsRecentlyUpdated => (DateTime.UtcNow - UpdatedAt).TotalHours <= 24;
    }
}