using AccuPay.Core.Interfaces.Excel;
using AccuPay.Utilities.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccuPay.Core.Services.Imports.Salaries
{
    public class SalaryRowRecord : IExcelRowRecord
    {
        public int LineNumber { get; set; }

        [ColumnName("Employee No")]
        public string EmployeeNo { get; set; }

        [ColumnName("Effective From")]
        public DateTime? EffectiveFrom { get; set; }

        [ColumnName("Basic Salary")]
        public Decimal? BasicSalary { get; set; }

        [ColumnName("Allowance Salary")]
        public Decimal? AllowanceSalary { get; set; }
    }
}