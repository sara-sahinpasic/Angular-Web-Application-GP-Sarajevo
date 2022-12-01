using Domain.Entities.Korisnici;

namespace Domain.Abstractions.Interfaces.Korisnici;

public interface IRegistracijskiTokenRepository
{
    public Task<RegistracijskiToken?> Create(Korisnik user);
    public Task<RegistracijskiToken?> Create(Guid userId);
    public Task<RegistracijskiToken?> GetInactiveByTokenString(string tokenString);
    public Task<RegistracijskiToken?> Update(RegistracijskiToken registracijskiToken);
    public bool Delete(string tokenString);
    public bool Delete(RegistracijskiToken tokenString);
}
