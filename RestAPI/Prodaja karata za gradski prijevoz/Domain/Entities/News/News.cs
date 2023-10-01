using Domain.Abstractions.Classes;
using Domain.Entities.Users;

namespace Domain.Entities.News
{
    public sealed class News : Entity
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
