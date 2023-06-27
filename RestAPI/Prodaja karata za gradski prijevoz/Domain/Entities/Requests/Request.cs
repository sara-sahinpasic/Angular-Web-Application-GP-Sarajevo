using Domain.Abstractions.Classes;
using Domain.Enums.Request;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Requests
{
    public sealed class Request :Entity
    {
        public Guid RequestTypeId { get; set; } = Guid.Parse(RequestTypes.Employed.ToString());

        [NotMapped]
        public RequestTypes RequestType
        {
            get
            {
                return RequestTypes.From(RequestTypeId.ToString());
            }
            set
            {
                RequestTypeId=Guid.Parse(value.ToString());
            }
        }
        public DateTime DateCreated { get; set; }
        public bool Approved { get; set; }
        public string DocumentLink { get; set; }
        public Guid UserId { get; set; }
    }
}
