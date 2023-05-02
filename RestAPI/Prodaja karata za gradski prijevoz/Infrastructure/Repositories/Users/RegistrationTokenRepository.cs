using Application.Services.Abstractions.Interfaces.Repositories.Users;
using Domain.Entities.Users;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Users;

public sealed class RegistrationTokenRepository : GenericRepository<RegistrationToken>, IRegistrationTokenRepository
{
    public RegistrationTokenRepository(DataContext dataContext) : base(dataContext) { }

    public async Task DeleteAsync(string tokenString, CancellationToken cancellationToken)
    {
        RegistrationToken? registrationToken = await GetAll()
            .Where(token => token.Token == tokenString)
            .FirstOrDefaultAsync(cancellationToken);

        Delete(registrationToken);
    }

    public Task<RegistrationToken?> GetByTokenStringAsync(string tokenString, CancellationToken cancellationToken)
    {
        return GetAll().FirstOrDefaultAsync(token => token.Token == tokenString, cancellationToken);
    }

    public Task<RegistrationToken?> GetInactiveByTokenStringAsync(string tokenString, CancellationToken cancellationToken)
    {
        Task<RegistrationToken?> tokenTask = GetAll()
            .Include(r => r.User)
            .FirstOrDefaultAsync(x => x.Token == tokenString && !x.IsExpired && !x.IsActivated, cancellationToken);

        return tokenTask;
    }
}
