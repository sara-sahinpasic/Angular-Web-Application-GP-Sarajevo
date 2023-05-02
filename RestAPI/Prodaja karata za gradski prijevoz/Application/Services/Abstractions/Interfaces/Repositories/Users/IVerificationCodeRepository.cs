using Domain.Entities.Users;

namespace Application.Services.Abstractions.Interfaces.Repositories.Users;

public interface IVerificationCodeRepository : IGenericRepository<VerificationCode>
{
    // todo: maybe it is better to store these codes in RAM? on hold sprint 1
    Task<VerificationCode?> GetByUserIdAndCodeAsync(Guid userId, int verificationCode, CancellationToken cancellationToken);
    Task<User> GetUserByVerificationCodeAsync(int verificationCode, CancellationToken cancellationToken); // todo: find a way to know if the user is logged in via backend: on hold sprint 1
    Task<VerificationCode?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}
