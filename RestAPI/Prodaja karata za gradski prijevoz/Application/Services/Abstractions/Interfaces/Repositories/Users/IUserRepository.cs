using Domain.Entities.Users;

namespace Application.Services.Abstractions.Interfaces.Repositories.Users;

public interface IUserRepository : IGenericRepository<User>
{
    public Task<User?> GetByEmailAsync(string email);
}
