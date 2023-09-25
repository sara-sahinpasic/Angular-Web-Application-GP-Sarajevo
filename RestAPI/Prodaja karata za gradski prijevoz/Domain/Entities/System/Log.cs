using Domain.Abstractions.Classes;

namespace Domain.Entities.System;

public sealed class Log : Entity
{
    public string Level { get; set; } = null!;
    public string Message { get; set; } = null!;
    public DateTime Date { get; set; }
}
