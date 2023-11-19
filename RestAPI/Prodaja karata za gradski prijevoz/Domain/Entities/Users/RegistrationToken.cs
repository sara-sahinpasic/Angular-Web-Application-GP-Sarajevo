using Domain.Abstractions.Classes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Users;

public sealed class RegistrationToken : Entity
{
    [Required]
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    [Required]
    public string Token { get; set; } = null!;
    public bool IsExpired { get; set; }
    public bool IsActivated { get; set; }
    public DateTime CreatedDate { get; set; }
}
