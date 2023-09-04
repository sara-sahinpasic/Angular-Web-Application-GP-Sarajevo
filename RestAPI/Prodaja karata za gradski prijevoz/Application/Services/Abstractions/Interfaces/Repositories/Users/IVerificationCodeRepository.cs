using Domain.Entities.Users;

namespace Application.Services.Abstractions.Interfaces.Repositories.Users;

public interface IVerificationCodeRepository : IGenericRepository<VerificationCode>
{
    Task<VerificationCode?> GetByUserIdAndCodeAsync(Guid userId, int verificationCode, CancellationToken cancellationToken);
    Task<User> GetUserByVerificationCodeAsync(int verificationCode, CancellationToken cancellationToken);
    Task<VerificationCode?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}
