using Domain.Abstractions.Classes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Users;

public sealed class VerificationCode : Entity
{
    [Required]
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    [Required]
    public int Code { get; set; }

    [Required]
    public DateTime DateCreated { get; set; }
    
    [Required]
    public DateTime DateExpiring { get; set; }
    public bool Activated { get; set; }
}
