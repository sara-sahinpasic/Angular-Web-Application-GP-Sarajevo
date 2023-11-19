using System.ComponentModel.DataAnnotations;

namespace Presentation.DTO.User;

public sealed class UserRegistrationRequestDto
{
    [Required]
    public string FirstName { get; set; } = null!;
    [Required]
    public string LastName { get; set; } = null!;
    [Required, EmailAddress]
    public string Email { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
    [Required]
    public DateTime DateOfBirth { get; set; }
    [Required]
    public string? PhoneNumber { get; set; } = null!;
    public string? Address { get; set; }
}