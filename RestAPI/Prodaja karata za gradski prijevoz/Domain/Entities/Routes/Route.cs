using Domain.Abstractions.Classes;
using Domain.Entities.Stations;
using Domain.Entities.Vehicles;

namespace Domain.Entities.Routes;

public sealed class Route : Entity
{
    public Guid StartStationId { get; set; }
    public Station StartStation { get; set; } = null!;
    public Guid EndStationId { get; set; }
    public Station EndStation { get; set; } = null!;
    public TimeSpan TimeOfDeparture { get; set; }
    public TimeSpan TimeOfArival { get; set; }
    public Guid VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;
    public bool Active { get; set; }
    public bool ActiveOnHolidays { get; set; }
    public bool ActiveOnWeekends { get; set; }
}
