using Domain.Abstractions.Classes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Routes;

public sealed class Holiday : Entity
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public DateTime Date { get; set; }
}
