using Domain.Abstractions.Classes;
using Domain.Entities.Korisnici;

namespace Domain.Entities.Utisci;

public sealed class Utisak : Entity
{
    public string? Opis { get; set; }
    public DateTime DatumKreiranja { get; set; }
    public int Ocjena { get; set; }
    public Guid KorisnikId { get; set; }
    public Korisnik Korisnik { get; set; }
}
