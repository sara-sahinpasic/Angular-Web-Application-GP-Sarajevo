using Domain.Entities.Driver;

namespace Application.Services.Abstractions.Interfaces.Driver;

public interface IMalfunctionService
{
    public Task SendMalfunctionNotification(Malfunction malfunction, CancellationToken cancellationToken);
}
