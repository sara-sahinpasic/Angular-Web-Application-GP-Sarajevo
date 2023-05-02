using Domain.Abstractions.Classes;

namespace Domain.Entities.Users;

public sealed class Role : Entity
{
    public string Name { get; set; } = null!;
}

