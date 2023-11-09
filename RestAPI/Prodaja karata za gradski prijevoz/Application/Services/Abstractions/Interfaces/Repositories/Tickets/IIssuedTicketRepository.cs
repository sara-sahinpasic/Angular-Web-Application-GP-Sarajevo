using Domain.Entities.Tickets;
using Domain.Enums.OrderBy;
using System.Linq.Expressions;

namespace Application.Services.Abstractions.Interfaces.Repositories.Tickets;

public interface IIssuedTicketRepository : IGenericRepository<IssuedTicket>
{
    public Task<List<T>> GetByUserIdAsync<T>(Guid userId, Expression<Func<IssuedTicket, T>> selector, Expression<Func<IssuedTicket, object>>? orderBy = null, OrderBy order = OrderBy.Ascending, CancellationToken cancellationToken = default);
    public Task<IssuedTicket[]> GetUserIssuedTicketsForPurchaseHistoryReportAsync(Guid userId, CancellationToken cancellationToken = default);
    public Task<bool> HasUserPurchasedAnyTicketAsync(Guid userId, CancellationToken cancellationToken = default);
    public Task<ICollection<IssuedTicket>> GetIssuedTicketsForDateAsync(DateTime date, CancellationToken cancellationToken = default);
    public Task<ICollection<IssuedTicket>> GetIssuedTicketsForPeriodAsync(int month, int year, CancellationToken cancellationToken = default);
}
