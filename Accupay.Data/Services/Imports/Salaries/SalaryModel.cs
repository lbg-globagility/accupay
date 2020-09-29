using System;
using System.Collections.Generic;
using System.Text;

namespace AccuPay.Data.Services.Imports.Salaries
{
    public class SalaryModel
    {
        public string EmployeeNo { get; set; }
        public string EmployeeName { get; set; }

        public DateTime? EffectiveFrom { get; set; }

        public Decimal? BasicSalary { get; set; }

        public Decimal? AllowanceSalary { get; set; }

        public string Remarks { get; set; }
    }
}