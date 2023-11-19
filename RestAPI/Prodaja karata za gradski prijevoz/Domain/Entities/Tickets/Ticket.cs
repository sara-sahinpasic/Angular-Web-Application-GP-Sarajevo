using Domain.Abstractions.Classes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Tickets;

public sealed class Ticket : Entity
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public double Price { get; set; }
    public bool Active { get; set; }
}
