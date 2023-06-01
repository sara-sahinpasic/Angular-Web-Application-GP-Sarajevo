namespace Domain.Enums;

public abstract class StringEnum
{
    private readonly string _value;

    public StringEnum(string value) => _value = value;

    public override string ToString() => _value;
}
