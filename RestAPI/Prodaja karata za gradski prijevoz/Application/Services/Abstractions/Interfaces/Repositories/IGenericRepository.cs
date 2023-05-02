using Domain.Abstractions.Classes;

namespace Application.Services.Abstractions.Interfaces.Repositories;

public interface IGenericRepository<TEntity> where TEntity : Entity
{
    IQueryable<TEntity> GetAll();
    Task<TEntity?> GetByIdAsync(Guid Id, CancellationToken cancellationToken);
    public Guid? Create(TEntity? entity);
    public bool Delete(TEntity? entity);
    public void Update(TEntity? entity);
}
