using AccuPay.Data.Interfaces.Excel;
using AccuPay.Utilities.Attributes;
using System;

namespace AccuPay.Data.Services.Imports.Overtimes
{
    public class OvertimeRowRecord : IExcelRowRecord
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