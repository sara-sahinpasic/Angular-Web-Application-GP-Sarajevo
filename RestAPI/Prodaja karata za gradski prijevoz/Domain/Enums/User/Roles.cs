namespace Domain.Enums.User;

// typesafe custom enum
public class Roles : StringEnum
{
    public static readonly Roles Admin = new("f9fefebe-9bec-480f-bbec-431f72b14995");
    public static readonly Roles User = new("f3206708-33aa-4be0-b4ae-cb6cc10005cf");
    public static readonly Roles Driver = new("7690f2ef-fb11-479a-84ba-e951ea07f341");

    private Roles(string value) : base(value) { }

    private static List<Roles> List() => new List<Roles>() { Admin, User, Driver };

    public static Roles From(string value)
    {
        Roles? enumValue = List().FirstOrDefault(enumValue => string.Equals(enumValue.ToString(), value, StringComparison.OrdinalIgnoreCase));

        if (enumValue is null)
        {
            throw new ArgumentOutOfRangeException(value, "The provided value is not part of the enum values");
        }

        return enumValue;
    }
    
}