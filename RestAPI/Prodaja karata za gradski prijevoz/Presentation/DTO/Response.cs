namespace Presentation.DTO;

public sealed class Response
{
    public string Message { get; set; } = string.Empty;
    public object? Data { get; set; }
}
