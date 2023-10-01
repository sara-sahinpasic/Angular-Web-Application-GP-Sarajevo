using System.ComponentModel.DataAnnotations;

namespace Presentation.DTO.News
{
    public class NewsDto
    {
        public Guid? Id { get; set; }
        
        [Required]
        public string Title { get; set; } = null!;
        
        [Required]
        public string Content { get; set; } = null!;
        public DateTime? Date { get; set; }
        public string? CreatedBy { get; set; }
    }
}
