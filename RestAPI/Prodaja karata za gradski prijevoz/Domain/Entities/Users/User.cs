using Domain.Abstractions.Classes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Users;
// todo: make mandatory fields required: sprint 2
public sealed class User : Entity
{
    public Guid RoleId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; } = null!;
    public string? PasswordHash { get; set; }
    public byte[]? PasswordSalt { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string? Address { get; set; }
    public DateTime RegistrationDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public bool Active { get; set; }
}

