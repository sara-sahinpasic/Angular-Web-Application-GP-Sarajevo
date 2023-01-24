using Domain.Entities.Korisnici;

namespace Application.Services.Abstractions.Interfaces.Email;

public interface IEmailService
{
    Task SendNoReplyMail(Korisnik to, string subject, string content, CancellationToken cancellationToken);
    Task SendRegistrationMail(Korisnik user, string token, CancellationToken cancellationToken);
    Task SendLoginVerificationMail(Korisnik user, int verificationCode, CancellationToken cancellationToken);
}
