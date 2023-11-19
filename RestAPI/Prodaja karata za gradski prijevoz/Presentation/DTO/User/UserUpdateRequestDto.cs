using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModels;

public sealed class UserUpdateRequestDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string FirstName { get; set; } = null!;
    [Required]
    public string LastName { get; set; } = null!;
    [Required]
    public string PhoneNumber { get; set; } = null!;
    public string? Address { get; set; } = null!;
    public IFormFile? ProfileImageFile { get; set; }
    public string? Password { get; set; }
}
