namespace Presentation.DTO.Holiday;

public sealed class HolidayResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime Date { get; set; }
}
