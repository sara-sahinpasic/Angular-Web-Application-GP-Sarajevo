using Domain.Entities.Payment;

namespace Application.Services.Abstractions.Interfaces.Repositories.Payment;

public interface ITaxRepository : IGenericRepository<Tax>
{
    Task<Tax?> GetTaxByNameAsync(string taxName, CancellationToken cancellationToken = default);
    Task<Tax?> GetActiveAsync(CancellationToken cancellationToken = default);
}
