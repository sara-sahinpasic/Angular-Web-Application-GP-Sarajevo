namespace Application.DataClasses.User;

public sealed class RegisterResult
{
    public bool IsAccountActivationRequired { get; set; }
    public Guid UserId { get; set; }
}
