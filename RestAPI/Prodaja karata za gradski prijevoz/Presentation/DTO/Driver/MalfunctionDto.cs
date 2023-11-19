using System.ComponentModel.DataAnnotations;

namespace Presentation.DTO.Driver
{
    public class MalfunctionDto
    {
        [Required]
        public string Description { get; set; } = null!;
        [Required]
        public DateTime DateOfMalufunction { get; set; }
        public bool Fixed { get; set; }
        [Required]
        public Guid VehicleId { get; set; }
    }
}
