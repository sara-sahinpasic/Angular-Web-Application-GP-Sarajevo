using Domain.Abstractions.Classes;
using Domain.Entities.Stations;
using Domain.Entities.Vehicles;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Routes;

public sealed class Route : Entity
{
    [Required]
    public Guid StartStationId { get; set; }
    public Station StartStation { get; set; } = null!;
    [Required]
    public Guid EndStationId { get; set; }
    public Station EndStation { get; set; } = null!;
    [Required]
    public TimeSpan TimeOfDeparture { get; set; }
    [Required]
    public TimeSpan TimeOfArrival { get; set; }
    [Required]
    public Guid VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;
    public bool Active { get; set; }
    public bool ActiveOnHolidays { get; set; }
    public bool ActiveOnWeekends { get; set; }
}
