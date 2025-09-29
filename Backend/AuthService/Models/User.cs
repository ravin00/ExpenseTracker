using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Models;

[Table("Users")]
public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    [Column(TypeName = "varchar(50)")]
    public required string Username { get; set; }

    [Required]
    [StringLength(100)]
    [Column(TypeName = "varchar(100)")]
    public required string Email { get; set; }

    [Required]
    [StringLength(255)]
    public required string PasswordHash { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? LastLoginAt { get; set; }

    public bool IsActive { get; set; } = true;
}