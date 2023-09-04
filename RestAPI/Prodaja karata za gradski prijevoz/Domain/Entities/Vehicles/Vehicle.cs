using Domain.Abstractions.Classes;

namespace Domain.Entities.Vehicles;

public sealed class Vehicle : Entity
{
    public int Number { get; set; }
    public string RegistrationNumber { get; set; } = null!;
    public string Color { get; set; } = null!;
    public Guid ManufacturerId { get; set; }
    public Manufacturer Manufacturer { get; set; } = null!;
    public Guid VehicleTypeId { get; set; }
    public VehicleType VehicleType { get; set; } = null!;
    public int BuildYear { get; set; }
}
