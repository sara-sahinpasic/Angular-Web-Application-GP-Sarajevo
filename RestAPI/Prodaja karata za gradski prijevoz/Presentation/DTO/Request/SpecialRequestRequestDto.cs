using Microsoft.AspNetCore.Http;

namespace Presentation.DTO.Request;
public sealed class SpecialRequestRequestDto
{
    public Guid UserId { get; set; }
    public Guid UserStatusId { get; set; }
    public IFormFile Document { get; set; } = null!;
}
