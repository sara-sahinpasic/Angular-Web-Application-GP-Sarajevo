using Application.Services.Abstractions.Interfaces.Repositories.Tickets;
using Domain.Entities.Tickets;
using Infrastructure.Data;

namespace Infrastructure.Repositories.Tickets;

public sealed class TicketRepository : GenericRepository<Ticket>, ITicketRepository
{
    public TicketRepository(DataContext dataContext) : base(dataContext) {}
}
