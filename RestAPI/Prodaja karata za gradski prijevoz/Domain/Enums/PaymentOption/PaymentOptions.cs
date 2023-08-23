namespace Domain.Enums.PaymentOption;

public sealed class PaymentOptions : StringEnum
{
    public static readonly PaymentOptions Card = new("8e5264f5-0eea-4fae-9945-80d835583ba1");
    public static readonly PaymentOptions Mail = new("46536a11-f5b3-4505-a13a-e7d44dda9ae9");

    public PaymentOptions(string value) : base(value) {}

    private static List<PaymentOptions> List() => new() { Card, Mail };

    public static PaymentOptions From(string value)
    {
        PaymentOptions? enumValue = List().FirstOrDefault(enumValue => string.Equals(enumValue.ToString(), value, StringComparison.OrdinalIgnoreCase));

        if (enumValue is null)
        {
            throw new ArgumentOutOfRangeException(value, "The provided value is not part of the enum values");
        }

        return enumValue;
    }

    public static string GetPaymentOptionName(PaymentOptions paymentOption)
    {
        return paymentOption.ToString() switch
        {
            "8e5264f5-0eea-4fae-9945-80d835583ba1" => "Card",
            "46536a11-f5b3-4505-a13a-e7d44dda9ae9" => "Mail",
            _ => throw new ArgumentException($"Payment option with the Id of {paymentOption} is not a valid payment option", nameof(paymentOption)),
        };
    }
}
