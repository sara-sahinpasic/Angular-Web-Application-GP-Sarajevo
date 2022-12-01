using Domain.Entities.Korisnici;

namespace Application.Abstractions.Email;

public interface IEmailHandler
{
    Task SendNoReplyMail(Korisnik user, string subject, string content);
    Task SendRegistrationMail(Korisnik user);
}
