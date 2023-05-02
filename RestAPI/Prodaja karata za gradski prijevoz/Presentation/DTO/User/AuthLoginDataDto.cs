using System.ComponentModel.DataAnnotations;

namespace Presentation.DTO.User;

public sealed class AuthLoginDataDto
{
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public int Code { get; set; }
}
