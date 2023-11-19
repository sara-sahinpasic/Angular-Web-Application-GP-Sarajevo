using Domain.Abstractions.Classes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Vehicles;

public sealed class Manufacturer : Entity
{
    [Required]
    public string Name { get; set; } = null!;
}
