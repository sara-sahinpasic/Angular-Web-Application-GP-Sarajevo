namespace Presentation.DTO;

internal class Response<TDataType>
{
    public string Message { get; set; } = string.Empty;
    public TDataType? Data { get; set; }
}
