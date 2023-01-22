using Domain.Entities.Korisnici;

namespace Application.Services.Abstractions.Interfaces.Repositories.Korisnici;

public interface IKorisnikRepozitorij
{
    public Task<Korisnik> GetById(Guid id);
    public Task<Korisnik> GetByEmail(string email);
    public Task<Guid?> Create(Korisnik user);
    public bool Delete(Korisnik user);
    public Task<Korisnik> Update(Korisnik user);
}
