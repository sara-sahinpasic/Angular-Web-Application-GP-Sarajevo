using Domain.Abstractions.Classes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Stations;

public sealed class Station : Entity
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public DateTime DateCreated { get; set; } = DateTime.Now;
}
