using Domain.Entities.Invoices;
using Domain.Entities.Users;

namespace Application.Services.Abstractions.Interfaces.Checkout;

public interface ICheckoutService
{
    Task IssueTicketAsync(Invoice invoice, User user, CancellationToken cancellationToken = default);
    Task IssueInvoiceAsync(Invoice invoice, User user, CancellationToken cancellationToken = default);
}
