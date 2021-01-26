namespace AccuPay.Infrastructure.Reports
{
    public class ExcelReportColumn
    {
        public string Name { get; set; }
        public ColumnType Type { get; set; }
        public string Source { get; set; }
        public bool Optional { get; set; }

        public ExcelReportColumn(string name, string source, ColumnType type = ColumnType.Numeric, bool optional = false)
        {
            Name = name;
            Source = source;
            Type = type;
            Optional = optional;
        }

        public enum ColumnType
        {
            Text,
            Numeric
        }
    }
}
