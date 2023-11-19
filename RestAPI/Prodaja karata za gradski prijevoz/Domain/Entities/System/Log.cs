using Domain.Abstractions.Classes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.System;

public sealed class Log : Entity
{
    [Required]
    public string Level { get; set; } = null!;
    [Required]
    public string Message { get; set; } = null!;
    public string? InnerMessage { get; set; }
    [Required]
    public DateTime Date { get; set; }
}
