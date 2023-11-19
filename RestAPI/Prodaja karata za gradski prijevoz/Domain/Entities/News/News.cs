using Domain.Abstractions.Classes;
using Domain.Entities.Users;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.News
{
    public sealed class News : Entity
    {
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public string Content { get; set; } = null!;
        public DateTime Date { get; set; } = DateTime.Now;
        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
