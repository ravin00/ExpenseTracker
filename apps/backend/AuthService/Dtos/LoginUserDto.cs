using System.ComponentModel.DataAnnotations;

namespace AuthService.Dtos;

public class LoginUserDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Password is required")]
    public required string Password { get; set; }
}