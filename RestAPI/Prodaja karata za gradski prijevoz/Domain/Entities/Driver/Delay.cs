using Domain.Abstractions.Classes;
using Domain.Entities.Routes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Driver
{
    public class Delay : Entity
    {
        [Required]
        public string Reason { get; set; } = null!;
        [Required]
        public Guid RouteId { get; set; }
        public Route Route { get; set; } = null!;
        [Required]
        public int DelayAmount { get; set; }
    }
}
