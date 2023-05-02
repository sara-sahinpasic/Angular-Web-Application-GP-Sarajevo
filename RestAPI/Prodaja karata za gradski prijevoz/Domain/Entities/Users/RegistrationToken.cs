using Domain.Abstractions.Classes;

namespace Domain.Entities.Users;

public sealed class RegistrationToken : Entity
{
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public string? Token { get; set; }
    public bool IsExpired { get; set; }
    public bool IsActivated { get; set; }
    public DateTime CreatedDate { get; set; }
}
