namespace Presentation.DTO.Invoice
{
    public sealed class IssuedTicketHistoryDto
    {
        public string TicketName { get; set; }
        //public Guid RelationId { get; set; } ToDo: add later
        public double Price { get; set; }
        public DateTime IssuedDate { get; set; }
    }
}
