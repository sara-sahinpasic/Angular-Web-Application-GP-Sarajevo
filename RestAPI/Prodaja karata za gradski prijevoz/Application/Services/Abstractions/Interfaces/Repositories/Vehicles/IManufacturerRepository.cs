using Domain.Entities.Vehicles;

namespace Application.Services.Abstractions.Interfaces.Repositories.Vehicles;

public interface IManufacturerRepository : IGenericRepository<Manufacturer>
{
    public Task<bool> IsManufacturerRegisteredAsync(string name, CancellationToken cancellationToken = default);
}
