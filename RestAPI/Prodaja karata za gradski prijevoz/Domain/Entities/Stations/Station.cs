using Domain.Abstractions.Classes;

namespace Domain.Entities.Stations;

public sealed class Station : Entity
{
    public string Name { get; set; } = null!;
}
