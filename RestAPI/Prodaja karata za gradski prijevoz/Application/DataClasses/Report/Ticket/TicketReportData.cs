namespace Application.DataClasses.Report.Ticket;

public sealed class TicketReportData : ReportData<TicketReportRow>
{
    public double TotalSum { get; set; }
}
