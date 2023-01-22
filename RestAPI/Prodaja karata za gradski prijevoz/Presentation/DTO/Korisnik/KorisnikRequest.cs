using System.ComponentModel.DataAnnotations;

namespace Presentation.DTO.Korisnik;

public sealed class KorisnikRequest
{
    [Required]
    public string? Ime { get; set; }
    [Required]
    public string? Prezime { get; set; }
    [Required, EmailAddress]
    public string? Email { get; set; }
    [Required]
    public string? Password { get; set; }
    public DateTime DatumRodjenja { get; set; }
    public string? BrojTelefona { get; set; }
    public string? Adresa { get; set; }
    public DateTime DatumRegistracije { get; set; }
    public DateTime DatumIzmjena { get; set; }
}
