namespace Presentation.DTO.Admin.Ticket
{
    public class TicketDto
    {
        public string Name { get; set; } = null!;
        public double Price { get; set; }
        public bool Active { get; set; }
    }
}
