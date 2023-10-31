using Domain.Abstractions.Classes;

namespace Application.Services.Abstractions.Interfaces.Repositories;

public interface IGenericRepository<TEntity> where TEntity : Entity
{
    IQueryable<TEntity> GetAll();
    Task<TEntity?> GetByIdAsync(Guid id, IEnumerable<string>? includes = null, CancellationToken cancellationToken = default);
    Task<TEntity> GetByIdEnsuredAsync(Guid id, IEnumerable<string>? includes = null, CancellationToken cancellationToken = default);
    public Task<Guid?> CreateAsync(TEntity? entity, CancellationToken cancellationToken = default);
    public Task CreateRangeAsync(IReadOnlyCollection<TEntity> entities, CancellationToken cancellationToken = default);
    public Task DeleteAsync(TEntity? entity, CancellationToken cancellationToken = default);
    public Task UpdateAsync(TEntity? entity, CancellationToken cancellationToken = default);
}
