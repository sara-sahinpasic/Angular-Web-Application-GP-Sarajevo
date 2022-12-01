using Application.Helpers;
using Domain.Abstractions.Interfaces.Korisnici;
using Domain.Entities.Korisnici;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Korisnici;

public sealed class RegistracijskiTokenRepository : IRegistracijskiTokenRepository
{
    private readonly DataContext _dataContext;

    public RegistracijskiTokenRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<RegistracijskiToken?> Create(Korisnik user)
    {
        if (user is null)
            return null;

        RegistracijskiToken token = new RegistracijskiToken
        {
            Korisnik = user,
            Token = EncryptionDecryptionHelper.GenerateRegistrationToken(),
            Kreiran = DateTime.Now
        };

        await _dataContext.RegistrationTokens.AddAsync(token);
        await _dataContext.SaveChangesAsync();

        return token;
    }

    public Task<RegistracijskiToken?> Create(Guid userId)
    {
        throw new NotImplementedException();
    }

    public bool Delete(string tokenString)
    {
        throw new NotImplementedException();
    }

    public bool Delete(RegistracijskiToken tokenString)
    {
        throw new NotImplementedException();
    }

    public async Task<RegistracijskiToken?> GetInactiveByTokenString(string tokenString)
    {
        RegistracijskiToken? token = await _dataContext.RegistrationTokens
                .Include(r => r.Korisnik)
                .FirstOrDefaultAsync(x => x.Token == tokenString && !x.Istekao && !x.Aktiviran);

        return token;
    }

    public async Task<RegistracijskiToken?> Update(RegistracijskiToken? registracijskiToken)
    {
        if (registracijskiToken is null)
            return null;

        _dataContext.RegistrationTokens.Update(registracijskiToken);
        await _dataContext.SaveChangesAsync();

        return registracijskiToken;
    }
}
