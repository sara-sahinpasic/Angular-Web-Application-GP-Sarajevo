using Domain.Abstractions.Classes;

namespace Domain.Entities.Korisnici;

public sealed class VerificationCode : Entity
{
    public Guid UserId { get; set; }
    public Korisnik? User { get; set; }
    public int Code { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateExpiring { get; set; }
    public bool Activated { get; set; }
}
