using Domain.Abstractions.Classes;

namespace Domain.Entities.Tickets;

public sealed class Ticket : Entity
{
    public string Name { get; set; } = null!;
    public double Price { get; set; }
    public bool Active { get; set; }
}
