using Domain.Abstractions.Classes;

namespace Domain.Entities.Payment;

public sealed class Tax : Entity
{
    public string Name { get; set; }
    public double Percentage { get; set; }
    public bool Active { get; set; }
}
