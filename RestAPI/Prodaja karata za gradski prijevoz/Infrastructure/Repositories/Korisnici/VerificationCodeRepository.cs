using Application.Services.Abstractions.Interfaces.Repositories.Korisnici;
using Domain.Entities.Korisnici;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Korisnici;

public sealed class VerificationCodeRepository : IVerificationCodeRepository
{
    private readonly DataContext _dataContext;

    public VerificationCodeRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<bool> CreateAsync(VerificationCode verificationCode)
    {
        await _dataContext.AddAsync(verificationCode);
        await _dataContext.SaveChangesAsync();

        return true;
    }

    public async Task<VerificationCode?> GetByUserIdAndCode(Guid userId, int verificationCode)
    {
        return await _dataContext.VerificationCodes
            .Include(c => c.User)
            .Where(c => c.UserId == userId && c.Code == verificationCode)
            .FirstOrDefaultAsync();
    }

    public async Task<Korisnik?> GetUserByVerificationCode(int verificationCode)
    {
        var code = await _dataContext.VerificationCodes
            .Include(v => v.User)
            .FirstOrDefaultAsync(v => v.Code == verificationCode);
        
        return code?.User!;
    }

    public async Task<bool> UpdateAsync(VerificationCode verificationCode)
    {
        _dataContext.VerificationCodes.Update(verificationCode);
        await _dataContext.SaveChangesAsync();

        return true;
    }
}
