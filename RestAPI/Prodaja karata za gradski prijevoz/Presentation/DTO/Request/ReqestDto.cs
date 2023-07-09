using Microsoft.AspNetCore.Http;

namespace Presentation.DTO.Request
{
    public sealed class ReqestDto
    {
        public Guid UserId { get; set; }
        public Guid UserStatusId { get; set; }
        public DateTime DateCreated { get; set; }
        public IFormFile DocumentLink { get; set; } = null!;
    }
}
