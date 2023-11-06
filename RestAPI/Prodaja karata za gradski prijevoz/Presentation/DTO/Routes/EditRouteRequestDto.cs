using System.ComponentModel.DataAnnotations;

namespace Presentation.DTO.Routes;

public class EditRouteRequestDto : CreateRouteRequestDto
{
    [Required]
    public Guid Id { get; set; }
}
