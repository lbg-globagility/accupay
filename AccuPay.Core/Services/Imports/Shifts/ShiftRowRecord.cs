using AccuPay.Core.Interfaces.Excel;
using AccuPay.Utilities.Attributes;
using System;

namespace AccuPay.Core.Services.Imports
{
    public class ShiftRowRecord : IExcelRowRecord
    {
        [ColumnName("Employee No")]
        public string EmployeeNo { get; set; }

        [ColumnName("Effective Date From")]
        public DateTime StartDate { get; set; }

        [ColumnName("Effective Date To (Optional)")]
        public DateTime? EndDate { get; set; }

        [ColumnName("Time From")]
        public TimeSpan? StartTime { get; set; }

        [ColumnName("Time To")]
        public TimeSpan? EndTime { get; set; }

        [ColumnName("Break Start Time (Optional)")]
        public TimeSpan? BreakStartTime { get; set; }

        [ColumnName("Break Length (Optional)")]
        public decimal BreakLength { get; set; }

        [ColumnName("Offset (Optional) (true/false)")]
        public bool IsRestDay { get; set; }

        public int LineNumber { get; set; }
    }
}
