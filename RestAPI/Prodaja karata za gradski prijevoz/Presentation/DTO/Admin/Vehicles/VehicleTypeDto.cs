using System.ComponentModel.DataAnnotations;

namespace Presentation.DTO.Admin.Vehicles;

public sealed class VehicleTypeDto
{
    public Guid? Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;
}