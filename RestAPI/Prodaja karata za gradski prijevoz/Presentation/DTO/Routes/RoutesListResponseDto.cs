namespace Presentation.DTO.Routes;

public sealed class RoutesListResponseDto
{
    public Guid Id { get; set; }
    public string StartingLocation { get; set; } = "";
    public string EndingLocation { get; set; } = "";
    public string TimeOfDeparture { get; set; } = "";
    public string TimeOfArrival { get; set; } = "";
    public int VehicleNumber { get; set; }
    public bool ActiveOnWeekends { get; set; }
    public bool ActiveOnHolidays { get; set; }
    public bool Active { get; set; }
}
