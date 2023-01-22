using Domain.Entities.Korisnici;

namespace Application.Services.Abstractions.Interfaces.Email;

public interface IEmailService
{
    Task SendNoReplyMail(string to, string subject, string content, CancellationToken cancellationToken);
    Task SendNoReplyMail(Korisnik to, string subject, string content, CancellationToken cancellationToken);
    Task SendRegistrationMail(Korisnik user, string token, CancellationToken cancellationToken);
}
