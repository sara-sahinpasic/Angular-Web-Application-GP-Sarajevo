using Domain.Abstractions.Interfaces.Korisnici;
using Domain.Entities.Korisnici;
using Infrastructure.Data;

namespace Infrastructure.Repositories.Korisnici;

public sealed class KorisnikRepository : IKorisnikRepozitorij
{
    private readonly DataContext _dataContext;

    public KorisnikRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<Guid?> Create(Korisnik user) //todo: handle exceptions
    {
        if (user is null)
            return null;

        await _dataContext.Users.AddAsync(user);
        await _dataContext.SaveChangesAsync();

        return user.Id;
    }

    public bool Delete(Korisnik user)
    {
        throw new NotImplementedException();
    }

    public Task<Korisnik> GetByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public Task<Korisnik> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<Korisnik> Update(Korisnik user)
    {
        _dataContext.Users.Update(user);
        await _dataContext.SaveChangesAsync();

        return user;
    }
}
