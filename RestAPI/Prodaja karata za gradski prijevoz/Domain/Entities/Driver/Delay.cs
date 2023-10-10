using Domain.Abstractions.Classes;
using Domain.Entities.Routes;

namespace Domain.Entities.Driver
{
    public class Delay : Entity
    {
        public string Reason { get; set; }
        public Guid RouteId { get; set; }
        public Route Route { get; set; } = null!;
        public int DelayAmount { get; set; }
    }
}
