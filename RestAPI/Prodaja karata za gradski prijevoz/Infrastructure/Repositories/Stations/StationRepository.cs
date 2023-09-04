using Application.Services.Abstractions.Interfaces.Repositories.Stations;
using Domain.Entities.Stations;
using Infrastructure.Data;

namespace Infrastructure.Repositories.Stations;

public sealed class StationRepository : GenericRepository<Station>, IStationRepository
{
    public StationRepository(DataContext dataContext) : base(dataContext) {}
}
