using Domain.Abstractions.Classes;
using Domain.Entities.Routes;
using Domain.Entities.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Tickets;

public sealed class IssuedTicket : Entity
{
    [Required]
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    [Required]
    public Guid TicketId { get; set; }
    public Ticket Ticket { get; set; } = null!;
    [Required]
    public DateTime ValidFrom { get; set; }
    [Required]
    public DateTime ValidTo { get; set; }
    [Required]
    public DateTime IssuedDate { get; set; }
    [NotMapped]
    public int Amount { get; set; }
    [Required]
    public Guid RouteId { get; set; }
    public Route Route { get; set; } = null!;    
}
