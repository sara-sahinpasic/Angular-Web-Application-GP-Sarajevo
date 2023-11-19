using System.ComponentModel.DataAnnotations;

namespace Presentation.DTO.Review
{
    public sealed class ReviewDto
    {
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        [Required]
        [Range(1, 5)]
        public int Score { get; set; }
        [Required]
        public Guid UserId { get; set; }
    }
}
