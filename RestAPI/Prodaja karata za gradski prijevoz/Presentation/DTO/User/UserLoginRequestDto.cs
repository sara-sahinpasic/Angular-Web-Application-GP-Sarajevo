using System.ComponentModel.DataAnnotations;

namespace Presentation.DTO.User;

public sealed class UserLoginRequestDto
{
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
}
