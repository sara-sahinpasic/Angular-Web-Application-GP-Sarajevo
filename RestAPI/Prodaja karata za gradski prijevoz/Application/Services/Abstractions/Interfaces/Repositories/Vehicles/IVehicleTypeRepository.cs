using Domain.Entities.Vehicles;

namespace Application.Services.Abstractions.Interfaces.Repositories.Vehicles;

public interface IVehicleTypeRepository : IGenericRepository<VehicleType>
{
    public Task<bool> IsVehicleTypeRegisteredAsync(string name, CancellationToken cancellationToken = default);

}
