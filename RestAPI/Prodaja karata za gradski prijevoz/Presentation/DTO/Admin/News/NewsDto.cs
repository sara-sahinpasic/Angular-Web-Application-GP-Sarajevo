namespace Presentation.DTO.Admin.News
{
    public class NewsDto
    {
        public string Title { get; set; } 
        public string Content { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public Guid UserId { get; set; }
    }
}
