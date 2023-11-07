using System.ComponentModel.DataAnnotations;

namespace Presentation.DTO.Holiday;

public sealed class HolidayRequestDto
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public DateTime Date { get; set; }
}
