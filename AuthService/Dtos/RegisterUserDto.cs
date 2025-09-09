namespace AuthService.Dtos;

public class RegisterUserDto
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}