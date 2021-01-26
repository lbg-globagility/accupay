namespace AccuPay.Core.Entities
{
    public class SystemInfo
    {
        public const string DesktopVersion = "desktop.version";

        // private constructor with property parameters is needed by ef core
        private SystemInfo(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public string Value { get; }
    }
}
