using Domain.Entities.Tickets;
using Domain.Enums.OrderBy;
using System.Linq.Expressions;

namespace Application.Services.Abstractions.Interfaces.Repositories.Tickets;

public interface IIssuedTicketRepository : IGenericRepository<IssuedTicket>
{
    Task<List<T>> GetByUserIdAsync<T>(Guid userId, Expression<Func<IssuedTicket, T>> selector, Expression<Func<IssuedTicket, object>>? orderBy = null, OrderBy order = OrderBy.Ascending, CancellationToken cancellationToken = default);
    Task<IssuedTicket[]> GetUserIssuedTicketsForPurchaseHistoryReportAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> HasUserPurchasedAnyTicketAsync(Guid userId, CancellationToken cancellationToken = default);
}
