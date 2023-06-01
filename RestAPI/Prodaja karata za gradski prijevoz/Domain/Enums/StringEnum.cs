using Domain.Enums.Interfaces;

namespace Domain.Enums;

public abstract class StringEnum : ICustomEnum<StringEnum, string>
{
    private readonly string _value;

    public StringEnum(string value) => _value = value;

    public override string ToString() => _value;
}
