namespace Presentation.DTO.User.Request;

public sealed class UserRequestResponseDto
{
    public Guid RequestId { get; set; }
    public Guid UserStatusId { get; set; }
    public string UserStatusName { get; set; } = null!;
    public DateTime DateCreated { get; set; }
    public bool Approved { get; set; }
    public string DocumentFile { get; set; } = null!;
    public string DocumentType { get; set; } = null!;
}
