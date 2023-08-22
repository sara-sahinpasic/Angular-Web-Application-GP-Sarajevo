using Domain.Abstractions.Classes;

namespace Domain.Entities.Reviews;

public class Review : Entity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime? DateOfCreation { get; set; }
    public int Score { get; set; }
    public Guid UserId { get; set; }
}
