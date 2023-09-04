using Application.Services.Abstractions.Interfaces.Repositories.Vehicles;
using Domain.Entities.Vehicles;
using Infrastructure.Data;

namespace Infrastructure.Repositories.Vehicles;

public sealed class VehicleTypeRepository : GenericRepository<VehicleType>, IVehicleTypeRepository
{
    public VehicleTypeRepository(DataContext dataContext) : base(dataContext) {}
}
