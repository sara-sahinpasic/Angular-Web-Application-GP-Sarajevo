using Application.Services.Abstractions.Interfaces.Repositories.Vehicles;
using Domain.Entities.Vehicles;
using Infrastructure.Data;

namespace Infrastructure.Repositories.Vehicles;

public sealed class ManufacturerRepository : GenericRepository<Manufacturer>, IManufacturerRepository
{
    public ManufacturerRepository(DataContext dataContext) : base(dataContext) {}
}
