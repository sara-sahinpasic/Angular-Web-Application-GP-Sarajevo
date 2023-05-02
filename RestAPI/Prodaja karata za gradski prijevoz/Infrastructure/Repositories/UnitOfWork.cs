using Application.Services.Abstractions.Interfaces.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _dataContext;

    public UnitOfWork(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public Task CommitAsync(CancellationToken cancellationToken)
    {
        return _dataContext.SaveChangesAsync(cancellationToken);
    }
}
