namespace Domain.Enums.Ticket;

public sealed class Tickets : StringEnum
{
    public static readonly Tickets OneWay = new("929CB30E-AE11-4653-8F20-41C3B39102BD");
    public static readonly Tickets Return = new("5DD86ADC-50BE-4DB6-98C5-46C4A582B61A");
    public static readonly Tickets Kids = new("FB272AC2-6C72-40FC-A425-96DA10A0077C");
    public static readonly Tickets Day = new("B8EEC999-55FF-47B5-9CE0-CFEDCABADBA6");

    private Tickets(string value) : base(value) { }

    private static List<Tickets> List() => new List<Tickets>() { OneWay, Return, Kids, Day};

    public static Tickets From(string value)
    {
        Tickets? enumValue = List().FirstOrDefault(enumValue => string.Equals(enumValue.ToString(), value, StringComparison.OrdinalIgnoreCase));

        if (enumValue is null)
        {
            throw new ArgumentOutOfRangeException(value, "The provided value is not part of the enum values");
        }

        return enumValue;
    }
}
