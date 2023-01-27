using Domain.Entities.Korisnici;

namespace Application.Services.Abstractions.Interfaces.SMS;

public interface ISMSService
{
    Task<bool> SendVerificationCode(Korisnik user);
}
