using Domain.Abstractions.Classes;

namespace Domain.Entities.Requests
{
    public sealed class RequestType :Entity
    {
        public string Name { get; set; }
    }
}
