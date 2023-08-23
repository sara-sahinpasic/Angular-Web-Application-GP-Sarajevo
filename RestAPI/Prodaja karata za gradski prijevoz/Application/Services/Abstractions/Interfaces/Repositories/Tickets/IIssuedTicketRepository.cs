using Domain.Entities.Tickets;

namespace Application.Services.Abstractions.Interfaces.Repositories.Tickets;

public interface IIssuedTicketRepository : IGenericRepository<IssuedTicket>
{
    Task<List<IssuedTicket>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IssuedTicket[]> GetUserIssuedTicketsForPurchaseHistoryReportAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> HasUserPurchasedAnyTicketAsync(Guid userId, CancellationToken cancellationToken = default);

}
