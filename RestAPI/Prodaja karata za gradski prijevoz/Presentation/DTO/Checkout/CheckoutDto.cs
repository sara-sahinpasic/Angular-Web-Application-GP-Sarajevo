using System.ComponentModel.DataAnnotations;

namespace Presentation.DTO.Checkout;

public sealed class CheckoutDto
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public Guid TicketId { get; set; }

    [Required]
    public Guid PaymentMethodId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Required]
    public Guid RouteId { get; set; }
    
    [Required]
    public DateTime Date { get; set; }
}
