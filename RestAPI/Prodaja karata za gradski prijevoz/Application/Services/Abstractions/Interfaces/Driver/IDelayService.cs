using Domain.Entities.Driver;

namespace Application.Services.Abstractions.Interfaces.Driver;

public interface IDelayService
{
    public Task SendDelayNotification(Delay delay, CancellationToken cancellationToken);
}
