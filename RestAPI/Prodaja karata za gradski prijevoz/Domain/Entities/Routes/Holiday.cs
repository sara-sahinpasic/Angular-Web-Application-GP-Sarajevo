using Domain.Abstractions.Classes;

namespace Domain.Entities.Routes;

public sealed class Holiday : Entity
{
    public string Name { get; set; } = null!;
    public DateTime Date { get; set; }
}
