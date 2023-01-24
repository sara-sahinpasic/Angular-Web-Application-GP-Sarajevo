using Domain.Entities.Korisnici;

namespace Application.Services.Abstractions.Interfaces.Repositories.Korisnici;

public interface IRegistracijskiTokenRepository
{
    public Task<Guid?> Create(RegistracijskiToken registracijskiToken);
    public Task<RegistracijskiToken?> GetInactiveByTokenString(string tokenString);
    public Task<RegistracijskiToken?> Update(RegistracijskiToken registracijskiToken);
    public bool Delete(string tokenString);
    public bool Delete(RegistracijskiToken tokenString);
}
