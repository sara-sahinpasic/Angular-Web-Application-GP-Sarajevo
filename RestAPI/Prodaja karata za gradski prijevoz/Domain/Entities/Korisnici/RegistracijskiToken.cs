using Domain.Abstractions.Classes;

namespace Domain.Entities.Korisnici;

public sealed class RegistracijskiToken : Entity
{
    public Guid KorisnikId { get; set; }
    public Korisnik? Korisnik { get; set; }
    public string? Token { get; set; }
    public bool Istekao { get; set; }
    public bool Aktiviran { get; set; }
    public DateTime Kreiran { get; set; }
}
