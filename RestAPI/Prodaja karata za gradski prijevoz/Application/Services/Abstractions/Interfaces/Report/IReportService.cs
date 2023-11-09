using Application.DataClasses.Report.Route;
using Application.DataClasses.Report.Ticket;

namespace Application.Services.Abstractions.Interfaces.Report;

public interface IReportService
{
    public Task<byte[]> GetPurchaseHistoryReportAsync(Guid userId, CancellationToken cancellationToken = default);
    public Task<TicketReportData> GetTicketReportForDateAsync(DateTime date, CancellationToken cancellationToken = default);
    public Task<TicketReportData> GetTicketReportForPeriodAsync(int month, int year, CancellationToken cancellationToken = default);
    public Task<RouteReportData?> GetRouteWithMostPurchasedTicketsForPeriodAsync(int month, int year, CancellationToken cancellationToken = default);
}
