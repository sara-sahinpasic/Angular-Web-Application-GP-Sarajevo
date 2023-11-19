using System.ComponentModel.DataAnnotations;

namespace Presentation.DTO.Admin.Vehicles
{
    public class VehicleDto
    {
        [Required]
        public int Number { get; set; }
        [Required]
        public string RegistrationNumber { get; set; } = null!;
        [Required]
        public string Color { get; set; } = null!;
        [Required]
        public Guid ManufacturerId { get; set; }
        [Required]
        public Guid VehicleTypeId { get; set; }
        [Required]
        public int BuildYear { get; set; }
        [Required]
        public Guid VehicleId { get; set; }
    }
}
