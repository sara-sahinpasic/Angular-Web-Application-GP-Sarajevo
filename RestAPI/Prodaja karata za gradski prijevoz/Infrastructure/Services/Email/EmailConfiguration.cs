namespace Infrastructure.Services.Email;

public sealed class EmailConfiguration
{
    public string? SMTP { get; set; }
    public int Port { get; set; }
    public bool UseTLS { get; set; } = false;
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? From { get; set; }
}
