using Domain.Entities.Payment;

namespace Application.Services.Abstractions.Interfaces.Repositories.Payment;

public interface IPaymentMethodRepository : IGenericRepository<PaymentMethod>
{
    Task<bool> DoesPaymentMethodExistAsync(Guid paymentMethodId, CancellationToken cancellationToken = default);
}
