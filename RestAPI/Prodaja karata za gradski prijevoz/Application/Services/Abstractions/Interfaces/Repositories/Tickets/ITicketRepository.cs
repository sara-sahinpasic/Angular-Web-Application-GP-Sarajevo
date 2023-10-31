using Domain.Entities.Tickets;

namespace Application.Services.Abstractions.Interfaces.Repositories.Tickets;

public interface ITicketRepository : IGenericRepository<Ticket>
{
    IQueryable<Ticket> GetAll(bool includeInactive);
    Task<bool> DoesTicketExistAsync(Guid ticketId, CancellationToken cancellationToken = default);
    Task<bool> DoesNameTicketExistAsync(string name, CancellationToken cancellationToken = default);
}
