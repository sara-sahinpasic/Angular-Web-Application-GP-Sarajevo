namespace Domain.Enums.User
{
    public class Statuses : StringEnum
    {
        public static readonly Statuses Student = new("056b4a11-96b3-413c-a323-0cef9a5680c2");
        public static readonly Statuses Pensioner = new("e6957173-7aa6-4fcb-9dc0-2fc20c20ecae");
        public static readonly Statuses Employed = new("4c0170aa-cf87-46bd-88a6-bab3687f48b6");
        public static readonly Statuses Unemployed = new("9647c387-b0fb-4336-9434-079249f37e76");


        private Statuses(string value) : base(value) { }

        private static List<Statuses> List() => new List<Statuses>() { Student, Pensioner, Employed, Unemployed };

        public static Statuses From(string value)
        {
            Statuses? enumValue = List().FirstOrDefault(enumValue => string.Equals(enumValue.ToString(), value, StringComparison.OrdinalIgnoreCase));

            if (enumValue is null)
            {
                throw new ArgumentOutOfRangeException(value, "The provided value is not part of the enum values");
            }

            return enumValue;
        }
    }
}