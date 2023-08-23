using Application.Services.Abstractions.Interfaces.Repositories.Payment;
using Domain.Entities.Payment;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Payment;

public sealed class PaymentMethodRepository : GenericRepository<PaymentMethod>, IPaymentMethodRepository
{
    public PaymentMethodRepository(DataContext dataContext) : base(dataContext) {}

    public Task<bool> DoesPaymentMethodExistAsync(Guid paymentMethodId, CancellationToken cancellationToken = default)
    {
        return GetAll().AnyAsync(method => method.Id == paymentMethodId);
    }
}
