using Domain.Abstractions.Classes;

namespace Domain.Entities.Payment;

public sealed class PaymentOption : Entity
{
    public string Name { get; set; } = null!;
}
