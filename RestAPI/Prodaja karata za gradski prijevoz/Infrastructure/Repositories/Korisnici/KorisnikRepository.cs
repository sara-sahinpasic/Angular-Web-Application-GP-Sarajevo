using Application.Services.Abstractions.Interfaces.Repositories.Korisnici;
using Domain.Entities.Korisnici;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Korisnici;

public sealed class KorisnikRepository : IKorisnikRepozitorij
{
    private readonly DataContext _dataContext;

    public KorisnikRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<Guid?> Create(Korisnik user) //todo: exception handler bug fix it
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

    public Task<Korisnik?> GetByEmail(string email)
    {
        return _dataContext.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
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
