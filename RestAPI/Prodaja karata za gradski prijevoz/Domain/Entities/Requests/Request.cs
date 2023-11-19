using Domain.Abstractions.Classes;
using Domain.Entities.Users;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Requests
{
    public sealed class Request : Entity
    {
        [Required]
        public Guid UserStatusId { get; set; }
        public Status UserStatus { get; set; } = null!;
        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        [Required]
        public DateTime DateCreated { get; set; }
        public bool Approved { get; set; }
        [Required]
        public string DocumentLink { get; set; } = null!;
        public string? RejectionReason { get; set; }
        public bool Active { get; set; }
    }
}
