using Application.Services.Abstractions.Interfaces.Repositories.Tickets;
using Domain.Entities.Tickets;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Tickets;

public sealed class IssuedTicketRepository : GenericRepository<IssuedTicket>, IIssuedTicketRepository
{
    public IssuedTicketRepository(DataContext dataContext) : base(dataContext) {}

    public Task<List<IssuedTicket>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return GetAll().Where(issuedTicket => issuedTicket.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public Task<IssuedTicket[]> GetUserIssuedTicketsForPurchaseHistoryReportAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return GetAll().Include(issuedTicket => issuedTicket.Ticket)
            .Where(issuedTicket => issuedTicket.UserId == userId)
            .Select(issuedTicket => new IssuedTicket
            {
                Ticket = issuedTicket.Ticket,
                IssuedDate = issuedTicket.IssuedDate
            })
            .ToArrayAsync(cancellationToken);
    }

    public Task<bool> HasUserPurchasedAnyTicketAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return GetAll()
             .Where(issuedTicket => issuedTicket.UserId == userId)
             .AnyAsync();
    }    
}
