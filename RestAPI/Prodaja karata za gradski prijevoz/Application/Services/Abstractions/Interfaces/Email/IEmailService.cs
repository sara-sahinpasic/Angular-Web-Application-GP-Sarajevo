using Domain.Entities.Invoices;
using Domain.Entities.Tickets;
using Domain.Entities.Users;

namespace Application.Services.Abstractions.Interfaces.Email;

public interface IEmailService
{
    Task SendNoReplyMailAsync(User to, string subject, string content, CancellationToken cancellationToken);
    Task SendRegistrationMailAsync(User user, string token, CancellationToken cancellationToken);
    Task SendLoginVerificationMailAsync(User user, int verificationCode, CancellationToken cancellationToken);
    Task SendIssuedTicketsAsync(User user, Invoice invoice, CancellationToken cancellationToken);
    Task SendInvoiceAsync(User user, Invoice invoice, CancellationToken cancellationToken);
}
