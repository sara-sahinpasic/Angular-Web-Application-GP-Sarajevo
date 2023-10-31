using Application.Services.Abstractions.Interfaces.Repositories.Tickets;
using Domain.Entities.Tickets;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Tickets;

public sealed class TicketRepository : GenericRepository<Ticket>, ITicketRepository
{
    public TicketRepository(DataContext dataContext) : base(dataContext) { }

    public IQueryable<Ticket> GetAll(bool includeInactive)
    {
        IQueryable<Ticket> query = GetAll();

        if (!includeInactive)
        {
            query = query.Where(ticket => ticket.Active);
        }
        
        return query;
    }

    public Task<bool> DoesNameTicketExistAsync(string name, CancellationToken cancellationToken = default)
    {
        return GetAll().AnyAsync(ticket => ticket.Name.Trim().ToLower() == name.Trim().ToLower(), cancellationToken);
    }

    public Task<bool> DoesTicketExistAsync(Guid ticketId, CancellationToken cancellationToken = default)
    {
        return GetAll().AnyAsync(ticket => ticket.Id == ticketId, cancellationToken);
    }
}
