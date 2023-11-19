namespace Application.DataClasses.User;

public sealed class UserClaims
{
    public Guid Id { get; set; }
    public string Role { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Address { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string PhoneNumber { get; set; } = null!;
}
