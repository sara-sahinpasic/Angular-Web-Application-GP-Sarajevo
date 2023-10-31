using Domain.Entities.Users;

namespace Application.Services.Abstractions.Interfaces.Repositories.Users;

public interface IRegistrationTokenRepository : IGenericRepository<RegistrationToken>
{
    public Task<RegistrationToken?> GetInactiveByTokenStringAsync(string tokenString, CancellationToken cancellationToken);
    public Task<RegistrationToken?> GetByTokenStringAsync(string tokenString, CancellationToken cancellationToken);
    public Task DeleteByTokenStringAsync(string tokenString, CancellationToken cancellationToken);
}
