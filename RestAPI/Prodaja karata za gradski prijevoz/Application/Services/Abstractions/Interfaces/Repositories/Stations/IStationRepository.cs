using Domain.Entities.Stations;

namespace Application.Services.Abstractions.Interfaces.Repositories.Stations;

public interface IStationRepository : IGenericRepository<Station>
{
    public Task<IReadOnlyList<Station>> GetRoutedStationsAsync(Guid startStationId, CancellationToken cancellationToken = default);
    public Task<bool> IsStationPartOfAnyRoute(Guid stationId, CancellationToken cancellationToken = default);
    public Task<bool> IsStationPartOfAnyRoute(Station station, CancellationToken cancellationToken = default);

}
