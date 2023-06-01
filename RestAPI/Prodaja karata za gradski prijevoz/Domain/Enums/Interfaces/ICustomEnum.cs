namespace Domain.Enums.Interfaces;

public interface ICustomEnum<TEnumType, TValue> where TEnumType : class
{
    public static TEnumType From(TValue value) => throw new NotImplementedException(); // todo: maybe there is a better way of forcing a static implementation
}
