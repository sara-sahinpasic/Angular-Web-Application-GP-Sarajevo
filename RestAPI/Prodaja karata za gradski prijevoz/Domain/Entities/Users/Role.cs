using Domain.Abstractions.Classes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Users;

public sealed class Role : Entity
{
    [Required]
    public string Name { get; set; } = null!;
}

