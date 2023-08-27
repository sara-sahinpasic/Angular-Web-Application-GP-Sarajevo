using System.ComponentModel.DataAnnotations;

namespace Presentation.DTO.User;

public sealed class UserLoginRequestDto
{
    public string Email { get; set; }
    
    public string Password { get; set; }
}
