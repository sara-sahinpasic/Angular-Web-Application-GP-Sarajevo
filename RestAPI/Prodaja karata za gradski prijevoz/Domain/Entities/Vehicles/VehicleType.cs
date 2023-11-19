using Domain.Abstractions.Classes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Vehicles;

public sealed class VehicleType : Entity
{
    [Required]
    public string Name { get; set; } = null!; 
}
