namespace Domain.ViewModels;

public sealed class UserUpdateRequestDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string? Address { get; set; }
    public Guid Id { get; set; }
}
