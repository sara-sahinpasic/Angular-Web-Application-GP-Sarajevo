using Domain.Abstractions.Classes;

namespace Domain.Entities.Korisnici;

public sealed class Uloga : Entity
{
    public string Naziv { get; set; }
}

