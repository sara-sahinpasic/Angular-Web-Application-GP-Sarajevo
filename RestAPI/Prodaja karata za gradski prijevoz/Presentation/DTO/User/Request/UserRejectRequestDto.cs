using System.ComponentModel.DataAnnotations;

namespace Presentation.DTO.User.Request;

public sealed class UserRejectRequestDto
{
    [Required(ErrorMessage = "Morate unijeti razlog odbijanja.")]
    public string RejectionReason { get; set; } = null!;
}
