using Domain.Abstractions.Classes;

namespace Domain.Entities.Payment;

public sealed class PaymentMethod : Entity
{
    public string Name { get; set; } = null!;
}
