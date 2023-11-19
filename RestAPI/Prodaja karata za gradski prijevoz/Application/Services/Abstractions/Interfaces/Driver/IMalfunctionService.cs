using Domain.Entities.Driver;
using Domain.Entities.Vehicles;

namespace Application.Services.Abstractions.Interfaces.Driver;

public interface IMalfunctionService
{
    public Task SendMalfunctionNotification(Malfunction malfunction, CancellationToken cancellationToken);
    public Task SendFixedMalfunctionNotification(Vehicle vehicle, CancellationToken cancellationToken);
}
