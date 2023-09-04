namespace Presentation.DTO.Invoice
{
    public sealed class IssuedTicketHistoryDto
    {
        public string TicketName { get; set; } = null!;
        public string StartStationName { get; set; } = null!;
        public string EndStationName { get; set; } = null!;
        public double Price { get; set; }
        public DateTime IssuedDate { get; set; }
    }
}
