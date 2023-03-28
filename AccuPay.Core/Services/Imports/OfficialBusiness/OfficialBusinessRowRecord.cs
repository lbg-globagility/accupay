using AccuPay.Core.Interfaces.Excel;
using AccuPay.Utilities.Attributes;
using System;

namespace AccuPay.Core.Services.Imports.OfficialBusiness
{
    public class OfficialBusinessRowRecord : ExcelEmployeeRowRecord, IExcelRowRecord
    {
        public int LineNumber { get; set; }

        [ColumnName("Employee ID")]
        public string EmployeeNo { get; set; }

        [ColumnName("Start Date")]
        public DateTime? StartDate { get; set; }

        [ColumnName("Start Time")]
        public DateTime? StartTime { get; set; }

        [ColumnName("End Time")]
        public DateTime? EndTime { get; set; }
    }
}
