namespace Application.Services.Abstractions.Interfaces.Repositories;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken);
}
