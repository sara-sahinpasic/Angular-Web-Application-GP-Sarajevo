using Domain.Abstractions.Classes;
using Domain.Entities.Vehicles;

namespace Domain.Entities.Driver
{
    public class Malfunction : Entity
    {
        public string Description { get; set; }
        public DateTime DateOfMalufunction { get; set; }
        public bool Fixed { get; set; }
        public Guid VehicleId { get; set; }
        public Vehicle Vehicle { get; set; } = null!;
    }
}
