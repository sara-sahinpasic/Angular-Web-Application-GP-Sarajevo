using Domain.Entities.Users;

namespace Application.Services.Abstractions.Interfaces.Repositories.Users;

public interface IUserRepository : IGenericRepository<User>
{
    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken, string[]? includes = null);
    public Task<bool> IsUserRegisteredAsync(string email, CancellationToken cancellationToken = default);
}
