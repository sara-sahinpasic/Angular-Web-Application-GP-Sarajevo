using Application.Services.Abstractions.Interfaces.Repositories.Users;
using Domain.Entities.Users;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Users;

public sealed class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(DataContext dataContext) : base(dataContext) {}

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken, string[]? includes = null)
    {
        ArgumentNullException.ThrowIfNull(email, nameof(email));
        IQueryable<User> query = GetAll();

        if (includes is not null)
        {
            foreach (string item in includes)
            {
                query = query.Include(item);
            }
        }

        return query.FirstOrDefaultAsync(user => user.Email!.ToLower() == email.ToLower(), cancellationToken);
    }

    public Task<bool> IsUserRegisteredAsync(string email, CancellationToken cancellationToken = default)
    {
        return GetAll().AnyAsync(user => user.Email.ToLower() == email.ToLower());
    }
}
