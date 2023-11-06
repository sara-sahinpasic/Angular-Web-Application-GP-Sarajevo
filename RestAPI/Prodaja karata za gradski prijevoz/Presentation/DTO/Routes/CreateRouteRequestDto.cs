using System.ComponentModel.DataAnnotations;

namespace Presentation.DTO.Routes;

public class CreateRouteRequestDto
{
    [Required]
    public Guid StartStationId { get; set; }

    [Required]
    public Guid EndStationId { get; set; }

    [Required] 
    public string TimeOfDeparture { get; set; } = null!;
    
    [Required]
    public string TimeOfArrival { get; set; } = null!;
    
    [Required]
    public Guid VehicleId { get; set; }
    public bool ActiveOnHolidays { get; set; }
    public bool ActiveOnWeekends { get; set; }
    public bool Active { get; set; }
}