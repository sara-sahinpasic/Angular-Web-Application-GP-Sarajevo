using Domain.Abstractions.Classes;
using Domain.Enums.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Users;
// todo: make mandatory fields required: sprint 3
public sealed class User : Entity
{
    public Guid RoleId { get; set; } = Guid.Parse(Roles.User.ToString());

    [NotMapped]
    public Roles Role
    {
        get
        {
            return Roles.From(RoleId.ToString());
        }

        set 
        {
            Role = value;
            RoleId = Guid.Parse(value.ToString());
        }
    }
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

