using Domain.Entities.Korisnici;

namespace Application.Services.Abstractions.Interfaces.Repositories.Korisnici;

public interface IVerificationCodeRepository
{
    Task<bool> CreateAsync(VerificationCode verificationCode);
    Task<bool> UpdateAsync(VerificationCode verificationCode);
    Task DeleteAsync(VerificationCode verificationCode); // todo: maybe it is better to store these codes in RAM?
    Task<VerificationCode?> GetByUserIdAndCode(Guid userId, int verificationCode);
    Task<Korisnik?> GetUserByVerificationCode(int verificationCode); // todo: find a way to know if the user is logged in via backend
    Task<VerificationCode?> GetByUserIdAsync(Guid userId);
}
