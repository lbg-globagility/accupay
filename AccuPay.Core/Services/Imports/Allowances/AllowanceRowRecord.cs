using AccuPay.Core.Interfaces.Excel;
using AccuPay.Utilities.Attributes;
using System;

namespace AccuPay.Core.Services.Imports.Allowances
{
    public class AllowanceRowRecord : ExcelEmployeeRowRecord, IExcelRowRecord
    {
        public int LineNumber { get; set; }

        [ColumnName("EmployeeID")]
        public string EmployeeNo { get; set; }

        [ColumnName("Name of allowance")]
        public string AllowanceName { get; internal set; }

        [ColumnName("Effective start date")]
        public DateTime? EffectiveStartDate { get; set; }

        [ColumnName("Allowance frequency(Daily, Semi-monthly)")]
        public string AllowanceFrequency { get; set; }

        [ColumnName("Effective end date")]
        public DateTime? EffectiveEndDate { get; set; }

        [ColumnName("Allowance amount")]
        public decimal? Amount { get; set; }
    }
}
