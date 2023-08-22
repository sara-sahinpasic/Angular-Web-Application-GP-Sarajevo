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

    public Task<bool> HasUserPurchasedAnyTicketAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return GetAll()
             .Where(user => user.Id == userId)
             .AnyAsync();
    }    
}
