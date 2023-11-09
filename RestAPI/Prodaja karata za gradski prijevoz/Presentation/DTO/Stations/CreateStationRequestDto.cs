using System.ComponentModel.DataAnnotations;

namespace Presentation.DTO.Stations;

public sealed class StationRequestDto
{
    [Required(ErrorMessage = "Naziv ne smije biti prazan.")]
    public string Name { get; set; } = null!;
}
