using Application.Services.Abstractions.Interfaces.Repositories;
using Domain.Abstractions.Classes;
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

    public Task<TEntity?> GetByIdAsync(Guid Id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(Id, nameof(Id));

        return _dataContext.Set<TEntity>()
            .FirstOrDefaultAsync(entity => entity.Id == Id, cancellationToken);
    }

    public virtual Guid? Create(TEntity? entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        entity.Id = Guid.NewGuid();
        
        _dataContext.Add(entity);

        return entity.Id;
    }

    public virtual bool Delete(TEntity? entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        _dataContext.Remove(entity);

        return true;
    }

    public virtual void Update(TEntity? entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        _dataContext.Update(entity);
    }
}
