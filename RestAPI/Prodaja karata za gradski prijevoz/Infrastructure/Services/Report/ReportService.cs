using Application.DataClasses.Report.Route;
using Application.DataClasses.Report.Ticket;
using Application.Services.Abstractions.Interfaces.File;
using Application.Services.Abstractions.Interfaces.Report;
using Application.Services.Abstractions.Interfaces.Repositories.Tickets;
using Domain.Entities.Tickets;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Report;

public sealed class ReportService : IReportService
{
    private readonly IFileService _fileService;
    private readonly IIssuedTicketRepository _issuedTicketRepository;

    public ReportService(IFileService fileService, IIssuedTicketRepository issuedTicketRepository)
    {
        _fileService = fileService;
        _issuedTicketRepository = issuedTicketRepository;
    }

    public Task<byte[]> GetPurchaseHistoryReportAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return _fileService.GeneratePurchaseHistoryPDFAsync(userId, cancellationToken: cancellationToken);
    }

    public async Task<RouteReportData?> GetRouteWithMostPurchasedTicketsForPeriodAsync(int month, int year, CancellationToken cancellationToken = default)
    {
        RouteReportData? routeReportData = await _issuedTicketRepository.GetAll()
            .Where(issuedTicket => issuedTicket.IssuedDate.Month == month && issuedTicket.IssuedDate.Year == year)
            .GroupBy(issuedTicket => issuedTicket.RouteId)
            .Select(group => new RouteReportData
            {
                StartStation = group.First().Route.StartStation.Name,
                EndStation = group.First().Route.EndStation.Name,
                QuantityOfTicketsBought = group.Count()
            })
            .OrderByDescending(routeReport => routeReport.QuantityOfTicketsBought)
            .FirstOrDefaultAsync(cancellationToken);

        return routeReportData;
    }

    public async Task<TicketReportData> GetTicketReportForDateAsync(DateTime date, CancellationToken cancellationToken = default)
    {
        ICollection<IssuedTicket> issuedTickets = await _issuedTicketRepository.GetIssuedTicketsForDateAsync(date, cancellationToken: cancellationToken);
        TicketReportData ticketReportData = GenerateTicketReportData(issuedTickets);

        return ticketReportData;
    }


    public async Task<TicketReportData> GetTicketReportForPeriodAsync(int month, int year, CancellationToken cancellationToken = default)
    {
        ICollection<IssuedTicket> issuedTickets = await _issuedTicketRepository.GetIssuedTicketsForPeriodAsync(month, year, cancellationToken);
        TicketReportData ticketReportData = GenerateTicketReportData(issuedTickets);

        return ticketReportData;
    }
    private static TicketReportData GenerateTicketReportData(ICollection<IssuedTicket> issuedTickets)
    {
        List<TicketReportRow> ticketReportRows = new();

        foreach (IssuedTicket issuedTicket in issuedTickets)
        {
            ticketReportRows.Add(new()
            {
                CardType = issuedTicket.Ticket.Name,
                QuantitySold = issuedTicket.Amount
            });
        }

        double totalSum = issuedTickets.Sum(issuedTicket => issuedTicket.Ticket.Price * issuedTicket.Amount);

        TicketReportData ticketReportData = new()
        {
            Data = ticketReportRows,
            TotalSum = Math.Round(totalSum, 2)
        };

        return ticketReportData;
    }
}
