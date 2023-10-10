namespace Presentation.DTO.Driver
{
    public class DelayDto
    {
        public string Reason { get; set; }
        public Guid RouteId { get; set; }
        public int DelayAmount { get; set; }
    }
}
