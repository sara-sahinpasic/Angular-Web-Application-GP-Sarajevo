using Application.Services.Abstractions.Interfaces.Repositories.Vehicles;
using Domain.Entities.Vehicles;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Vehicles;

public sealed class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
{
    public VehicleRepository(DataContext dataContext) : base(dataContext) { }

    public Task<bool> IsVehicleRegisteredAsync(string registrationNumber, CancellationToken cancellationToken = default)
    {
        return GetAll().AnyAsync(vehicle => vehicle.RegistrationNumber.Trim().ToLower() == registrationNumber.Trim().ToLower());
    }
}
