using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos;

public class UserRegistrationDto
{
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; init; } = default!;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = default!;

    public string? UserName { get; init; }
}
