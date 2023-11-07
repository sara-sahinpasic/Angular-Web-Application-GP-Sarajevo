using System.ComponentModel.DataAnnotations;

namespace Presentation.DTO.Review
{
    public sealed class ReviewDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        [Range(1, 5)]
        public int Score { get; set; }
        public Guid UserId { get; set; }
    }
}
