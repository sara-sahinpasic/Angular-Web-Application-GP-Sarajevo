using Domain.Abstractions.Classes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Payment;

public sealed class Tax : Entity
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public double Percentage { get; set; }
    public bool Active { get; set; }
}
