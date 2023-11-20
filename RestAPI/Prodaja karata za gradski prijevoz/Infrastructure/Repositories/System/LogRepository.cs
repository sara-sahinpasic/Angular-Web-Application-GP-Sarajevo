using Application.Services.Abstractions.Interfaces.Repositories.System;
using Domain.Entities.System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Repositories.System;

public sealed class LogRepository : GenericRepository<Log>, ILogRepository
{
    private readonly DataContext _dataContext;
    public LogRepository(DataContext dataContext) : base(dataContext) 
    {
        _dataContext = dataContext;
    }

    public override Task<Guid?> CreateAsync(Log? entity, CancellationToken cancellationToken = default)
    {
        // clear all tracking if an error occurrs so that we can save the log to the DB and discard all other DB changes
        foreach (EntityEntry entry in _dataContext.ChangeTracker.Entries())
        {
            entry.State = EntityState.Detached;
        }

        return base.CreateAsync(entity, cancellationToken);
    }
}
