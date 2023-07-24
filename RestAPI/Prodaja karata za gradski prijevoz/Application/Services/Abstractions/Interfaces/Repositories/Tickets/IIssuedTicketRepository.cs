using Domain.Entities.Tickets;

namespace Application.Services.Abstractions.Interfaces.Repositories.Tickets;

public interface IIssuedTicketRepository : IGenericRepository<IssuedTicket>
{
    Task<List<IssuedTicket>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default); 
}
