using Domain.Abstractions.Classes;

namespace Domain.Entities.Relacije;

public sealed class Relacija : Entity
{
    public TimeOnly VrijemePolaska { get; set; }
    public TimeOnly VrijemeDolaska { get; set; }
    public bool Presijedanje { get; set; }
    public Guid PresijedanjeUVoziloId { get; set; }
    // todo: predsijedanjevozilo
    public Guid StartnaStanicaId { get; set; }
    public Stanica StartnaStanica { get; set; }
    public Guid ZavrsnaStanicaId { get; set; }
    public Stanica ZavrsnaStanica { get; set; }
    public Guid VoziloId { get; set; }
    // todo vozilo
    public Guid ZonaId { get; set; }
    public Zona Zona { get; set; }
    public bool Aktivna { get; set; }
}
