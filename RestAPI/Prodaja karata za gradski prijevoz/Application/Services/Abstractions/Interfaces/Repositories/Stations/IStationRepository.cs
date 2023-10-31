using Domain.Entities.Stations;

namespace Application.Services.Abstractions.Interfaces.Repositories.Stations;

public interface IStationRepository : IGenericRepository<Station>
{
    public Task<IReadOnlyList<Station>> GetRoutedStationsAsync(Guid startStationId,
        CancellationToken cancellationToken = default);
}
