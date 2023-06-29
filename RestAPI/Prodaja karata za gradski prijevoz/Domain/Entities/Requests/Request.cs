using Domain.Abstractions.Classes;
using Domain.Enums.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Requests
{
    public sealed class Request :Entity
    {
        public Guid UserStatusId { get; set; } = Guid.Parse(Statuses.Employed.ToString());

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
        public DateTime DateCreated { get; set; }
        public bool Approved { get; set; }
        public string DocumentLink { get; set; } = null!;
        public Guid UserId { get; set; }
    }
}
