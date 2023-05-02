using Application.Services.Abstractions.Interfaces.Repositories.Users;
using Domain.Entities.Users;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Users;

public sealed class VerificationCodeRepository : GenericRepository<VerificationCode>, IVerificationCodeRepository
{
    public VerificationCodeRepository(DataContext dataContext) : base(dataContext) { }

    
    public Task<VerificationCode?> GetByUserIdAndCodeAsync(Guid userId, int verificationCode, CancellationToken cancellationToken)
    {
        return GetAll()
            .Include(c => c.User)
            .Where(c => c.UserId == userId && c.Code == verificationCode)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<VerificationCode?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return GetAll().FirstOrDefaultAsync(vc => vc.UserId == userId, cancellationToken);
    }

    public async Task<User> GetUserByVerificationCodeAsync(int verificationCode, CancellationToken cancellationToken)
    {
        User user = await GetAll()
            .Include(v => v.User)
            .Where(code => code.Code == verificationCode)
            .Select(code => code.User)
            .FirstAsync(cancellationToken);
        
        return user;
    }
}
