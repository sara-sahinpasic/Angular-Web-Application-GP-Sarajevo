using Domain.Abstractions.Classes;
using Domain.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Tickets;

public sealed class IssuedTicket : Entity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid TicketId { get; set; }
    public Ticket Ticket { get; set; } = null!;
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public DateTime IssuedDate { get; set; }
    [NotMapped]
    public int Amount { get; set; }
    //public Guid RelationId { get; set; } // todo: implement later

}
