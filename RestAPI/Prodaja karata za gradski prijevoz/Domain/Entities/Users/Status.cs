using Domain.Abstractions.Classes;

namespace Domain.Entities.Users
{
    public sealed class Status : Entity
    {
        public string Name { get; set; }
        public double Discount { get; set; }
    }
}
