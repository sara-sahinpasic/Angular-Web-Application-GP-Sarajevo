using Application.Services.Abstractions.Interfaces.Repositories.Driver;
using Domain.Entities.Vehicles;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Driver.Malfunction
{
    public class MalfunctionRepository : GenericRepository<Domain.Entities.Driver.Malfunction>, IMalfunctionRepository
    {
        public MalfunctionRepository(DataContext dataContext) : base(dataContext) {}

        public Task<Domain.Entities.Driver.Malfunction> GetByVehicleAsync(Vehicle vehicle, CancellationToken cancellationToken)
        {
            return GetByVehicleIdAsync(vehicle.Id, cancellationToken);
        }

        public Task<Domain.Entities.Driver.Malfunction> GetByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return GetAll()
                .Where(malfunction => malfunction.VehicleId == vehicleId && !malfunction.Fixed)
                .FirstAsync(cancellationToken);
        }
    }
}
