namespace Presentation.DTO.Routes;

public sealed class SelectedRouteResponse
{
    public Guid Id { get; set; }
    public string StartingLocation { get; set; } = "";
    public string EndingLocation { get; set; } = "";
    public string Time { get; set; } = "";
    public DateTime Date { get; set; }
}
