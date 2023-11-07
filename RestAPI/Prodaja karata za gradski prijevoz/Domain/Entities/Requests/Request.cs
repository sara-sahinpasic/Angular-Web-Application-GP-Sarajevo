using Domain.Abstractions.Classes;
using Domain.Entities.Users;
using Domain.Enums.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Requests
{
    public sealed class Request : Entity
    {
        public Guid UserStatusId { get; set; }
        public Status UserStatus { get; set; } = null!;

        [NotMapped]
        public Statuses RequestType
        {
            get
            {
                return Statuses.From(UserStatusId.ToString());
            }
            set
            {
                UserStatusId = Guid.Parse(value.ToString());
            }
        }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public DateTime DateCreated { get; set; }
        public bool Approved { get; set; }
        public string DocumentLink { get; set; } = null!;
        public string? RejectionReason { get; set; }
        public bool Active { get; set; }
    }
}
