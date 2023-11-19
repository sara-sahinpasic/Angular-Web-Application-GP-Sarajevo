using System.ComponentModel.DataAnnotations;

namespace Presentation.DTO.Driver
{
    public class DelayDto
    {
        [Required]
        public string Reason { get; set; } = null!;
        [Required]
        public Guid RouteId { get; set; }
        [Required]
        public int DelayAmount { get; set; }
    }
}
