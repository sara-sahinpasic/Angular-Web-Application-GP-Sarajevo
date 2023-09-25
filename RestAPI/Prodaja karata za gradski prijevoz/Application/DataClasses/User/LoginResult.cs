namespace Application.DataClasses.User;

public class LoginResult
{
    public object? LoginData { get; set; } = null!;
    public bool IsTwoWayAuth { get; set; }
}
