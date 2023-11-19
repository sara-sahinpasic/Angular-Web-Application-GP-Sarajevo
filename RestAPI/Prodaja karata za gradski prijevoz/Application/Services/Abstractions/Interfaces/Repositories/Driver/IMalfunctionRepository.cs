using Domain.Entities.Driver;
using Domain.Entities.Vehicles;

namespace Application.Services.Abstractions.Interfaces.Repositories.Driver
{
    public interface IMalfunctionRepository : IGenericRepository<Malfunction>
    {
        public Task<Malfunction> GetByVehicleAsync(Vehicle vehicle, CancellationToken cancellationToken);
        public Task<Malfunction> GetByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken);

    }
}
