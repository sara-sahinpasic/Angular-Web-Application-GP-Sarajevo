using Domain.Abstractions.Classes;
using Domain.Entities.Vehicles;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Driver
{
    public class Malfunction : Entity
    {
        [Required]
        public string Description { get; set; } = null!;
        [Required]
        public DateTime DateOfMalufunction { get; set; }
        public bool Fixed { get; set; }
        [Required]
        public Guid VehicleId { get; set; }
        public Vehicle Vehicle { get; set; } = null!;
    }
}
