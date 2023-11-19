using Domain.Abstractions.Classes;
using Domain.Enums.User;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Users;

public sealed class User : Entity
{
    [Required]
    public Guid RoleId { get; set; } = Guid.Parse(Roles.User);
    public Role Role { get; set; } = null!;

    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;

    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string PasswordHash { get; set; } = null!;

    [Required]
    public byte[] PasswordSalt { get; set; } = null!;

    [Required]
    public DateTime DateOfBirth { get; set; }
    
    [Required]
    public string PhoneNumber { get; set; } = null!;
    public string? Address { get; set; }
    
    [Required]
    public DateTime RegistrationDate { get; set; }
    
    [Required]
    public DateTime ModifiedDate { get; set; }
    public bool Active { get; set; }
    public Guid? UserStatusId { get; set; }
    public Status? UserStatus { get; set; }
    public string? ProfileImagePath { get; set; }
    public DateTime? StatusExpirationDate { get; set; }
}

