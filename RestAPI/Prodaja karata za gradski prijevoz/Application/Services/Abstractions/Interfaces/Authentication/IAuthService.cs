using Domain.Entities.Korisnici;

namespace Application.Services.Abstractions.Interfaces.Authentication;

public interface IAuthService
{
    Task<Guid?> Register(Korisnik user, string password, CancellationToken cancellationToken);
    Task<bool> ActivateUserAccount(string tokenString);
    Task Logout(Korisnik user);
}
