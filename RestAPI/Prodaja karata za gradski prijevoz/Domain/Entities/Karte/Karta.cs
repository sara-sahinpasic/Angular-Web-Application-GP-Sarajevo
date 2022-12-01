using Domain.Abstractions.Classes;
using Domain.Entities.Relacije;

namespace Domain.Entities.Karte;

public sealed class Karta : Entity
{
    public int TipId { get; set; }
    public TipKarte? TipKarte { get; set; }
    public int ZonaId { get; set; }
    public Zona Zona { get; set; }
    public double Cijena { get; set; }
    public int BrojKoristenja { get; set; }
}
