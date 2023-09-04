using Domain.Entities.Routes;

namespace Application.Services.Abstractions.Interfaces.Repositories.Routes;

public interface IHolidayRepository : IGenericRepository<Holiday>
{
    public Task<bool> IsHolidayAsync(DateTime date, CancellationToken cancellationToken = default);
}
