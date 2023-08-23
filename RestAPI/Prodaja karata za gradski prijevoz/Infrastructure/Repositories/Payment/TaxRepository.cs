using Application.Services.Abstractions.Interfaces.Repositories.Payment;
using Domain.Entities.Payment;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Payment;

public sealed class TaxRepository : GenericRepository<Tax>, ITaxRepository
{
    public TaxRepository(DataContext dataContext) : base(dataContext)
    {
    }

    public Task<Tax?> GetActiveAsync(CancellationToken cancellationToken = default) => GetAll().Where(tax => tax.Active).SingleOrDefaultAsync(cancellationToken);

    public Task<Tax?> GetTaxByNameAsync(string taxName, CancellationToken cancellationToken = default) => GetAll().Where(tax => tax.Name == taxName).FirstOrDefaultAsync(cancellationToken);
}
