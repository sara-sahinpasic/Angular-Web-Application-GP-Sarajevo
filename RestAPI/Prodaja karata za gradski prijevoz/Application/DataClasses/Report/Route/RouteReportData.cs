namespace Application.DataClasses.Report.Route;

public sealed class RouteReportData
{
    public string StartStation { get; set; } = null!;
    public string EndStation { get; set; } = null!;
    public int QuantityOfTicketsBought { get; set; }
}
