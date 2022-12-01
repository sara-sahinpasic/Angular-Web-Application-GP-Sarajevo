using Domain.Abstractions.Classes;
using Domain.Entities.Korisnici;

namespace Domain.Entities.Karte;

public sealed class Potvrda : Entity
{
    public Guid KorisnikId { get; set; }
    public Korisnik? Korisnik { get; set; }
    public Guid KartaId { get; set; }
    public Karta? Karta { get; set; }
    public DateTime VaziOd { get; set; }
    public DateTime VaziDo { get; set; }
    public int IskoristenaBr { get; set; } = 0;
    public DateTime DatumKupovine { get; set; }
    public int Kolicina { get; set; }
}
