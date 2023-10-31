namespace Presentation.DTO.Admin.Vehicles;

internal sealed class VehicleListDto
{
    public Guid Id { get; set; }
    public int Number { get; set; }
    public string RegistrationNumber { get; set; }
    public string Color { get; set; }
    public VehicleTypeDto Type { get; set; }
    public ManufacturerDto Manufacturer { get; set; }
    public int BuildYear { get; set; }
}
