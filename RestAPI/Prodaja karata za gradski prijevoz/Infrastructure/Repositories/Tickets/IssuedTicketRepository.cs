using Application.Services.Abstractions.Interfaces.Repositories.News;
using Application.Services.Abstractions.Interfaces.Repositories.Tickets;
using Domain.Entities.Tickets;
using Domain.Enums.OrderBy;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Cryptography;

namespace Infrastructure.Repositories.Tickets;

public sealed class IssuedTicketRepository : GenericRepository<IssuedTicket>, IIssuedTicketRepository
{
    private readonly ITicketRepository _ticketRepository;

    public IssuedTicketRepository(DataContext dataContext, ITicketRepository ticketRepository) : base(dataContext)
    {
        _ticketRepository = ticketRepository;
    }

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

    public async Task<IEnumerable<IssuedTicket>> GetIssuedTicketsForDateAsync(DateTime date, CancellationToken cancellationToken = default)
    {
        IEnumerable<Ticket> tickets = await _ticketRepository.GetAll().AsNoTracking()
            .ToArrayAsync(cancellationToken);

        IDictionary<Guid, IssuedTicket> issuedTicketsForDate = await GetAll()
            .Where(issuedTicket => issuedTicket.IssuedDate.Date == date.Date)
            .GroupBy(issuedTicket => issuedTicket.TicketId)
            .Select(group => new IssuedTicket
            {
                TicketId = group.Key,
                Amount = group.Count(),
                Ticket = group.First(item => item.TicketId == group.Key).Ticket,
            })
            .AsNoTracking()
            .ToDictionaryAsync(key => key.TicketId, val => val, cancellationToken);

        ICollection<IssuedTicket> ticketsForReport = new List<IssuedTicket>();

        foreach (Ticket ticket in tickets)
        {
            if (issuedTicketsForDate.ContainsKey(ticket.Id))
            {
                ticketsForReport.Add(issuedTicketsForDate[ticket.Id]);
                continue;
            }

            ticketsForReport.Add(new IssuedTicket
            {
                Ticket = ticket,
                TicketId = ticket.Id,
                Amount = 0
            });
        }

        return ticketsForReport;
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
