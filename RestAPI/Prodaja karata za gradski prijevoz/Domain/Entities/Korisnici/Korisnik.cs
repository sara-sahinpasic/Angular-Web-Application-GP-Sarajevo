using Domain.Abstractions.Classes;

namespace Domain.Entities.Korisnici;

public sealed class Korisnik : Entity
{
    public Guid UlogaId { get; set; }
    public string? Ime { get; set; }
    public string? Prezime { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }
    public byte[]? PasswordSalt { get; set; }
    public DateTime DatumRodjenja { get; set; }
    public string? BrojTelefona { get; set; }
    public string? Adresa { get; set; }
    public DateTime DatumRegistracije { get; set; }
    public DateTime DatumIzmjena { get; set; }
    public bool Aktivan { get; set; }
}

