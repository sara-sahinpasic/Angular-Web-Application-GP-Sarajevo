using Application.Services.Abstractions.Interfaces.Repositories;
using Domain.Abstractions.Classes;
using Domain.Exceptions.Domain;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : Entity
{
    private readonly DataContext _dataContext;

    protected GenericRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public IQueryable<TEntity> GetAll()
    {
        return _dataContext.Set<TEntity>()
            .AsQueryable();
    }

    public Task<TEntity?> GetByIdAsync(Guid id, IEnumerable<string>? includes = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));
        cancellationToken.ThrowIfCancellationRequested();

        IQueryable<TEntity> query = _dataContext.Set<TEntity>();
        
        if (includes is not null)
        {
            query = includes.Aggregate(query, (current, item) => current.Include(item));
        }

        return query.FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    }
    
    public async Task<TEntity> GetByIdEnsuredAsync(Guid id, IEnumerable<string>? includes = null, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        TEntity? entity = await GetByIdAsync(id, includes, cancellationToken);

        if (entity is null)
        {
            throw new DomainException($"{typeof(TEntity)} with the id {id} doesn't exist.");
        }

        return entity;
    }

    public virtual async Task<Guid?> CreateAsync(TEntity? entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        cancellationToken.ThrowIfCancellationRequested();

        await _dataContext.AddAsync(entity, cancellationToken);

        return entity.Id;
    }

    public virtual Task CreateRangeAsync(IReadOnlyCollection<TEntity> entities, CancellationToken cancellationToken = default)
    {
        if (entities.Count == 0)
        {
            throw new ArgumentException("Cannot be empty.", nameof(entities));
        }

        cancellationToken.ThrowIfCancellationRequested();
        
        return _dataContext.AddRangeAsync(entities, cancellationToken);
    }

    public virtual Task DeleteAsync(TEntity? entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        cancellationToken.ThrowIfCancellationRequested();
        
        _dataContext.Remove(entity);

        return Task.CompletedTask;
    }

    public virtual Task UpdateAsync(TEntity? entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        cancellationToken.ThrowIfCancellationRequested();
        
        _dataContext.Update(entity);

        return Task.CompletedTask;
    }
}
