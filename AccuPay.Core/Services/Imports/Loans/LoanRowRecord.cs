using AccuPay.Core.Interfaces.Excel;
using AccuPay.Utilities.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace AccuPay.Core.Services.Imports.Loans
{
    public class LoanRowRecord : ExcelEmployeeRowRecord, IExcelRowRecord
    {
        [Required]
        [ColumnName("Employee ID")]
        public string EmployeeNo { get; set; }

        [Required]
        [ColumnName("Loan name")]
        public string LoanName { get; set; }

        [ColumnName("Loan number/code")]
        public string LoanNumber { get; set; }

        [Required]
        [ColumnName("Start date")]
        [DataType(DataType.Date, ErrorMessage = "Invalid Date Value")]
        public DateTime? StartDate { get; set; }

        [Required]
        [ColumnName("Total loan amount")]
        [Range(0, double.MaxValue, ErrorMessage = "Invalid Total loan amount")]
        public decimal? TotalLoanAmount { get; set; }

        [Required]
        [ColumnName("Loan balance")]
        [Range(0, double.MaxValue, ErrorMessage = "Invalid Loan balance")]
        public decimal? TotalLoanBalance { get; set; }

        [Required]
        [ColumnName("Deduction amount")]
        [Range(0, double.MaxValue, ErrorMessage = "Invalid Deduction amount")]
        public decimal? DeductionAmount { get; set; }

        [Required]
        [ColumnName("Deduction frequency(First half, End of the month, Per pay period)")]
        public string DeductionSchedule { get; set; }

        [ColumnName("Comments")]
        public string Comments { get; set; }

        public int LineNumber { get; set; }
    }
}
