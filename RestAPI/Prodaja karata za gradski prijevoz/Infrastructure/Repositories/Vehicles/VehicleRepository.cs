using Application.Services.Abstractions.Interfaces.Repositories.Vehicles;
using Domain.Entities.Vehicles;
using Infrastructure.Data;

namespace Infrastructure.Repositories.Vehicles;

public sealed class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
{
    public VehicleRepository(DataContext dataContext) : base(dataContext) {}
}
