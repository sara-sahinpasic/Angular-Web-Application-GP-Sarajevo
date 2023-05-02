using Application.Services.Abstractions.Interfaces.Repositories.Users;
using Domain.Entities.Users;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Users;

public sealed class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly DataContext _dataContext;

    public UserRepository(DataContext dataContext) : base(dataContext) 
    {
        _dataContext = dataContext;
    }

    public Task<User?> GetByEmailAsync(string email)
    {
        ArgumentNullException.ThrowIfNull(email, nameof(email));

        return GetAll().FirstOrDefaultAsync(u => u.Email!.ToLower() == email.ToLower());
    }
}
