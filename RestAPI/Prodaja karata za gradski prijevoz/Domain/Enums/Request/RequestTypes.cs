using Domain.Enums.User;

namespace Domain.Enums.Request
{
    public class RequestTypes : StringEnum
    {
        public static readonly RequestTypes Student = new("23a43e2c-ea65-4a33-9d5c-1195dfb72d43");
        public static readonly RequestTypes Employed = new("41a37a8d-4d5f-4353-988b-89cc2f7cb3db");
        public static readonly RequestTypes Pensioner = new("6309f61b-4a1d-4866-befb-ffef76f8b869");
        public static readonly RequestTypes Unemployed = new("6b989bc6-7314-4a5c-adca-f7b44ab3158a");

        private RequestTypes(string value) : base(value) { }

        private static List<RequestTypes> List() => new List<RequestTypes>() { Student, Employed, Pensioner, Unemployed };

        public static RequestTypes From(string value)
        {
            RequestTypes? enumValue = List().FirstOrDefault(enumValue => string.Equals(enumValue.ToString(), value, StringComparison.OrdinalIgnoreCase));

            if (enumValue is null)
            {
                throw new ArgumentOutOfRangeException(value, "The provided value is not part of the enum values");
            }

            return enumValue;
        }
    }
}
