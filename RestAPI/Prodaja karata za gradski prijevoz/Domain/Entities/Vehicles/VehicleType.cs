using Domain.Abstractions.Classes;

namespace Domain.Entities.Vehicles;

public sealed class VehicleType : Entity
{
    public string Name { get; set; } = null!; 
}
