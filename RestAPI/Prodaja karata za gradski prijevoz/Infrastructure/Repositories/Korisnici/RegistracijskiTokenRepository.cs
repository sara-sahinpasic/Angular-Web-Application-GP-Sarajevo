using Application.Services.Abstractions.Interfaces.Hashing;
using Application.Services.Abstractions.Interfaces.Repositories.Korisnici;
using Domain.Entities.Korisnici;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Korisnici;

public sealed class RegistracijskiTokenRepository : IRegistracijskiTokenRepository
{
    private readonly DataContext _dataContext;
    private readonly IHashingService _hashingService;

    public RegistracijskiTokenRepository(DataContext dataContext, IHashingService encryptionDecryptionService)
    {
        _dataContext = dataContext;
        _hashingService = encryptionDecryptionService;
    }

    public async Task<Guid?> Create(RegistracijskiToken token)
    {
        await _dataContext.RegistrationTokens.AddAsync(token);
        await _dataContext.SaveChangesAsync();

        return token.Id;
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
