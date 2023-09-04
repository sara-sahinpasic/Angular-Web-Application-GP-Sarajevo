using Application.Services.Abstractions.Interfaces.Repositories.Tickets;
using Domain.Entities.Tickets;
using Domain.Enums.OrderBy;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Tickets;

public sealed class IssuedTicketRepository : GenericRepository<IssuedTicket>, IIssuedTicketRepository
{
    public IssuedTicketRepository(DataContext dataContext) : base(dataContext) {}

    public Task<List<T>> GetByUserIdAsync<T>(
        Guid userId, 
        Expression<Func<IssuedTicket, T>> selector, 
        Expression<Func<IssuedTicket, object>>? orderBy = null,
        OrderBy order = OrderBy.Ascending,
        CancellationToken cancellationToken = default)
    {
        IQueryable<IssuedTicket> query = GetAll().Where(issuedTicket => issuedTicket.UserId == userId);

        if (orderBy is not null)
        {
            if (order == OrderBy.Ascending)
            {
                query = query.OrderBy(orderBy);
            }
            else
            {
                query = query.OrderByDescending(orderBy);
            }
        }

        return query.Select(selector)
                .ToListAsync(cancellationToken);
    }

    public Task<IssuedTicket[]> GetUserIssuedTicketsForPurchaseHistoryReportAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return GetAll().Include(issuedTicket => issuedTicket.Ticket)
            .Include(ticket => ticket.Route.StartStation)
            .Include(ticket => ticket.Route.EndStation)
            .OrderByDescending(ticket => ticket.IssuedDate)
            .Where(issuedTicket => issuedTicket.UserId == userId)
            .Select(issuedTicket => new IssuedTicket
            {
                Ticket = issuedTicket.Ticket,
                IssuedDate = issuedTicket.IssuedDate,
                Route = issuedTicket.Route
            })
            .ToArrayAsync(cancellationToken);
    }

    public Task<bool> HasUserPurchasedAnyTicketAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return GetAll()
             .Where(issuedTicket => issuedTicket.UserId == userId)
             .AnyAsync(cancellationToken);
    }    
}
