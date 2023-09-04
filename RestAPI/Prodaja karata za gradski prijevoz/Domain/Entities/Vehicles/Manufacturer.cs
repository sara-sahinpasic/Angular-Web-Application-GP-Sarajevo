using Domain.Abstractions.Classes;

namespace Domain.Entities.Vehicles;

public sealed class Manufacturer : Entity
{
    public string Name { get; set; } = null!;
}
