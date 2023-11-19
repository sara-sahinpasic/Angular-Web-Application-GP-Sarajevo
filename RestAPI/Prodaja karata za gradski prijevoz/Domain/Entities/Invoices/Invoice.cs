using Domain.Abstractions.Classes;
using Domain.Entities.Payment;
using Domain.Entities.Tickets;
using Domain.Entities.Users;
using Domain.Enums.PaymentOption;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Invoices;

public class Invoice : Entity
{
    [Required]
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    [Required]
    public Guid PaymentOptionId { get; set; }
    public virtual IEnumerable<IssuedTicket> IssuedTickets { get; set; } = new List<IssuedTicket>();

    [NotMapped]
    public PaymentOptions PaymentOption 
    { 
        get => PaymentOptions.From(PaymentOptionId.ToString()); 
        set => PaymentOptionId = new Guid(value.ToString());
    }
    [Required]
    public DateTime InvoicingDate { get; set; }
    [Required]
    public double Total { get; set; }
    [Required]
    public double TotalWithoutTax { get; set; }
    [Required]
    public Guid TaxId { get; set; }
    public Tax Tax { get; set; } = null!;
}
