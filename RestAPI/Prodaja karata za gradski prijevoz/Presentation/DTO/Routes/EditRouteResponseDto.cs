namespace Presentation.DTO.Routes;

internal sealed class EditRouteResponseDto
{
    public Guid StartStationId { get; set; }
    public Guid EndStationId { get; set; }
    public string TimeOfDeparture { get; set; } = null!;
    public string TimeOfArrival { get; set; } = null!;
    public Guid VehicleId { get; set; }
    public bool ActiveOnHolidays { get; set; }
    public bool ActiveOnWeekends { get; set; }
    public bool Active { get; set; }
}
