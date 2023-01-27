using Domain.Entities.Korisnici;

namespace Application.Services.Abstractions.Interfaces.Repositories.Korisnici;

public interface IVerificationCodeRepository
{
    Task<bool> CreateAsync(VerificationCode verificationCode);
    Task<bool> UpdateAsync(VerificationCode verificationCode);
    Task<VerificationCode?> GetByUserIdAndCode(Guid userId, int verificationCode);
    public Task<Korisnik?> GetUserByVerificationCode(int verificationCode); // todo: find a way to know if the user is logged in via backend

}
