using Application.Services.Abstractions.Interfaces.Repositories.Invoices;
using Domain.Entities.Invoices;
using Domain.Entities.Users;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Invoices;

public sealed class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
{
    public InvoiceRepository(DataContext dataContext) : base(dataContext) {}

    public Task<List<Invoice>> GetByUserAsync(User user, CancellationToken cancellationToken)
    {
        return GetByUserIdAsync(user.Id, cancellationToken);
    }

    public Task<List<Invoice>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return GetAll().Where(invoice => invoice.UserId == userId)
            .ToListAsync(cancellationToken);
    }
}
