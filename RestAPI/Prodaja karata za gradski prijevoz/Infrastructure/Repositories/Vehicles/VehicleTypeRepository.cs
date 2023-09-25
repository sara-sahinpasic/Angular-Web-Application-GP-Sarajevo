using Application.Services.Abstractions.Interfaces.Repositories.Vehicles;
using Domain.Entities.Vehicles;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Vehicles;

public sealed class VehicleTypeRepository : GenericRepository<VehicleType>, IVehicleTypeRepository
{
    public VehicleTypeRepository(DataContext dataContext) : base(dataContext) { }
    public Task<bool> IsVehicleTypeRegisteredAsync(string name, CancellationToken cancellationToken = default)
    {
        return GetAll().AnyAsync(vehicle => vehicle.Name.Trim().ToLower() == name.Trim().ToLower());
    }
}
