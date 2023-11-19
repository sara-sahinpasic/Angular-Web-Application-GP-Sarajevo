using Domain.Abstractions.Classes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Users
{
    public sealed class Status : Entity
    {
        [Required]
        public string Name { get; set; } = null!;
        
        [Required]
        public double Discount { get; set; }
    }
}
