using Domain.Abstractions.Classes;

namespace Domain.Entities.Relacije;

public sealed class Stanica : Entity
{
    public string? Naziv { get; set; }
    public Guid ZonaId { get; set; }
    // todo zona
}

