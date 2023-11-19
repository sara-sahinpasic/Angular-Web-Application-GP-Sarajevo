using Domain.Abstractions.Classes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Reviews;

public class Review : Entity
{
    [Required]
    public string Title { get; set; } = null!;
    [Required]
    public string Description { get; set; } = null!;
    [Required]
    public DateTime DateOfCreation { get; set; }
    [Required]
    public int Score { get; set; }
    [Required]
    public Guid UserId { get; set; }
}
