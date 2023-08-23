using Domain.Abstractions.Classes;
using Domain.Entities.Payment;
using Domain.Entities.Tickets;
using Domain.Entities.Users;
using Domain.Enums.PaymentOption;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Invoices;

public class Invoice : Entity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid PaymentOptionId { get; set; }
    public virtual IEnumerable<IssuedTicket> IssuedTickets { get; set; } = new List<IssuedTicket>();

    [NotMapped]
    public PaymentOptions PaymentOption 
    { 
        get => PaymentOptions.From(PaymentOptionId.ToString()); 
        set => PaymentOptionId = new Guid(value.ToString());
    }

    public DateTime InvoicingDate { get; set; }
    public double Total { get; set; }
    public double TotalWithoutTax { get; set; }
    public Guid TaxId { get; set; }
    public Tax Tax { get; set; } = null!;
}
