namespace Application.DataClasses.User;

public class LoginResult
{
    public string LoginData { get; set; } = null!;
    public bool IsTwoWayAuth { get; set; }
}
