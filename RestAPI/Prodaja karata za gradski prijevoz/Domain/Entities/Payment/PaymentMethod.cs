using Domain.Abstractions.Classes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Payment;

public sealed class PaymentMethod : Entity
{
    [Required]
    public string Name { get; set; } = null!;
}
