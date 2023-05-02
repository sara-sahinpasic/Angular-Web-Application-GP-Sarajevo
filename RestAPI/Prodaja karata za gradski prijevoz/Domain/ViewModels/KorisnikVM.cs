using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModels
{
    public class KorisnikVM
    {
        public string? Ime { get; set; }
        public string? Prezime { get; set; }
        public string BrojTelefona { get; set; }
        public string? Adresa { get; set; }
        public Guid Id { get; set; }
    }
}
