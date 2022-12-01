using Domain.Abstractions.Classes;
using Domain.Entities.Korisnici;

namespace Domain.Entities.Obavijesti;

public sealed class Obavijest : Entity
{
    public string? Opis { get; set; }
    public DateTime Datum { get; set; }
    public Guid KorisnikId { get; set; }
    public Korisnik? Korisnik { get; set; }
}
