using Application.Services.Abstractions.Interfaces.Repositories.Routes;
using Application.Services.Abstractions.Interfaces.Repositories.Stations;
using Domain.Entities.Stations;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Stations;

public sealed class StationRepository : GenericRepository<Station>, IStationRepository
{
    private readonly IRouteRepository _routeRepository;
    
    public StationRepository(DataContext dataContext, IRouteRepository routeRepository) : base(dataContext)
    {
        _routeRepository = routeRepository;
    }
    
    public Task<IReadOnlyList<Station>> GetRoutedStationsAsync(Guid startStationId, CancellationToken cancellationToken = default)
    {
        return _routeRepository.GetAll()
            .AsNoTracking()
            .Where(route => route.StartStationId == startStationId)
            .Select(route => new Station
            {
                Id = route.EndStationId,
                Name = route.EndStation.Name,
            })
            .Distinct()
            .ToListAsync(cancellationToken)
            .ContinueWith(list => (IReadOnlyList<Station>)list.Result, cancellationToken);
    }
}
