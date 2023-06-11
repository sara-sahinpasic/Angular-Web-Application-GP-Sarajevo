using Application.Services.Abstractions.Interfaces.Repositories.Users;
using Domain.Entities.Users;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Users;

public sealed class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(DataContext dataContext) : base(dataContext) {}

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(email, nameof(email));

        return GetAll().FirstOrDefaultAsync(user => user.Email!.ToLower() == email.ToLower(), cancellationToken);
    }
}
