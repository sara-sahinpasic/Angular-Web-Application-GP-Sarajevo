using Domain.Entities.Routes;
using System.Linq.Expressions;

namespace Application.Services.Abstractions.Interfaces.Repositories.Routes;

public interface IRouteRepository : IGenericRepository<Route>
{
    public Task<IEnumerable<T>> GetRoutesByDateAsync<T>(Guid startStationId, Guid endStationId, DateTime date, Expression<Func<Route, T>> selector, CancellationToken cancellationToken = default);
}
