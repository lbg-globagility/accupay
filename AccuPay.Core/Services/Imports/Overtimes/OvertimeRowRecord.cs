using AccuPay.Core.Interfaces.Excel;
using AccuPay.Utilities.Attributes;
using System;

namespace AccuPay.Core.Services.Imports.Overtimes
{
    public class OvertimeRowRecord : ExcelEmployeeRowRecord, IExcelRowRecord
    {
        public int LineNumber { get; set; }

        [ColumnName("EmployeeID")]
        public string EmployeeNo { get; set; }

        [ColumnName("Effective start date")]
        public DateTime? StartDate { get; set; }

        [ColumnName("Effective Start Time")]
        public DateTime? StartTime { get; set; }

        [ColumnName("Effective End Time")]
        public DateTime? EndTime { get; set; }
    }
}
