using Application.DataClasses.Report.Ticket;
using Application.Services.Abstractions.Interfaces.File;
using Application.Services.Abstractions.Interfaces.Report;
using Application.Services.Abstractions.Interfaces.Repositories.Tickets;
using Domain.Entities.Tickets;

namespace Infrastructure.Services.Report;

public sealed class ReportService : IReportService
{
    private readonly IFileService _fileService;
    private readonly IIssuedTicketRepository _isuedTicketRepository;

    public ReportService(IFileService fileService, IIssuedTicketRepository isuedTicketRepository)
    {
        _fileService = fileService;
        _isuedTicketRepository = isuedTicketRepository;
    }

    public Task<byte[]> GetPurchaseHistoryReportAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return _fileService.GeneratePurchaseHistoryPDFAsync(userId, cancellationToken: cancellationToken);
    }

    public async Task<TicketReportData> GetTicketReportForDateAsync(DateTime date, CancellationToken cancellationToken = default)
    {
        IEnumerable<IssuedTicket> issuedTickets = await _isuedTicketRepository.GetIssuedTicketsForDateAsync(date, cancellationToken: cancellationToken);
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
