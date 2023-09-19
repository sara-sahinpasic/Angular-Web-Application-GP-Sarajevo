using Domain.Entities.Vehicles;

namespace Application.Services.Abstractions.Interfaces.Repositories.Vehicles;

public interface IVehicleRepository : IGenericRepository<Vehicle>
{
    public Task<bool> IsVehicleRegisteredAsync(string registrationNumber, CancellationToken cancellationToken = default);

}
