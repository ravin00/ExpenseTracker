using System.ComponentModel.DataAnnotations;

namespace AuthService.Dtos
{
    public class UpdateProfileDto
    {
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores")]
        public string? Username { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(100, ErrorMessage = "Email must not exceed 100 characters")]
        public string? Email { get; set; }
    }
}
