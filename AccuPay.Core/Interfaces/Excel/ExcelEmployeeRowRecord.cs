using AccuPay.Utilities.Attributes;

namespace AccuPay.Core.Interfaces.Excel
{
    public abstract class ExcelEmployeeRowRecord
    {
        [ColumnName("EmployeeRowId")]
        public int EmployeeRowId { get; set; }
    }
}
