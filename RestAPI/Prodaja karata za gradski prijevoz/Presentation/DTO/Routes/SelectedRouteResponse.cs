namespace Presentation.DTO.Routes;

public sealed class SelectedRouteResponse
{
    public Guid Id { get; set; }
    public string StartingLocation { get; set; } = "";
    public string EndingLocation { get; set; } = "";
    public string TimeOfDeparture { get; set; } = "";
    public string TimeOfArrival { get; set; } = "";
    public int VehicleNumber { get; set; }
    public string VehicleType { get; set; } = "";
    public DateTime Date { get; set; }
}
