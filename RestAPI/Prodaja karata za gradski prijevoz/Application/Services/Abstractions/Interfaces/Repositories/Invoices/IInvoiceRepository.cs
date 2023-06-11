using Domain.Entities.Invoices;
using Domain.Entities.Users;

namespace Application.Services.Abstractions.Interfaces.Repositories.Invoices;

public interface IInvoiceRepository : IGenericRepository<Invoice>
{
    public Task<List<Invoice>> GetByUserAsync(User user, CancellationToken cancellationToken);
    public Task<List<Invoice>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}
