using Domain.Abstractions.Classes;
using Domain.Entities.Driver;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Vehicles;

public sealed class Vehicle : Entity
{
    [Required]
    public int Number { get; set; }

    [Required]
    public string RegistrationNumber { get; set; } = null!;

    [Required]
    public string Color { get; set; } = null!;

    [Required]
    public Guid ManufacturerId { get; set; }
    public Manufacturer Manufacturer { get; set; } = null!;

    [Required]
    public Guid VehicleTypeId { get; set; }
    public VehicleType VehicleType { get; set; } = null!;

    [Required]
    public int BuildYear { get; set; }
    public bool HasMalfunction { get; set; }
}
