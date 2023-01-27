using System.ComponentModel.DataAnnotations;

namespace Presentation.DTO.Korisnik;

public sealed class UserLoginRequestDto
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}
