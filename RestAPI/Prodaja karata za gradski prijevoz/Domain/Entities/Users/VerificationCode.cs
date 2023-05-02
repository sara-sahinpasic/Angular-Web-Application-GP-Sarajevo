using Domain.Abstractions.Classes;

namespace Domain.Entities.Users;

public sealed class VerificationCode : Entity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public int Code { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateExpiring { get; set; }
    public bool Activated { get; set; }
}
