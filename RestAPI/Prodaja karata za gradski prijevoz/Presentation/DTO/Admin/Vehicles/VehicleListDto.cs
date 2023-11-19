namespace Presentation.DTO.Admin.Vehicles;

internal sealed class VehicleListDto
{
    public Guid Id { get; set; }
    public int Number { get; set; }
    public string RegistrationNumber { get; set; } = null!;
    public string Color { get; set; } = null!;
    public VehicleTypeDto Type { get; set; } = null!;
    public ManufacturerDto Manufacturer { get; set; } = null!;
    public int BuildYear { get; set; }
    public bool HasMalfunction { get; set; }
}
