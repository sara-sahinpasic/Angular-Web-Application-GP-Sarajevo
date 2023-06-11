using Domain.Abstractions.Classes;
using Domain.Entities.Tickets;
using Domain.Entities.Users;
using Domain.Enums.PaymentOption;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Invoices;

public sealed class Invoice : Entity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    //public Guid RelationId { get; set; } todo: add later
    public Guid TicketId { get; set; }
    public Ticket Ticket { get; set; } = null!;
    public Guid PaymentOptionId { get; set; }

    [NotMapped]
    public PaymentOptions PaymentOption 
    { 
        get => PaymentOptions.From(PaymentOptionId.ToString()); 
        set => PaymentOptionId = new Guid(value.ToString());
    }
    public DateTime PurchaseDate { get; set; }
    public double Price { get; set; }
    public int Amount { get; set; }
}
