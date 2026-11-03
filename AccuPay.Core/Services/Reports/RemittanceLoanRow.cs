using System;

namespace AccuPay.Core.Services.Reports
{
    public class RemittanceLoanRow
    {
        public string EmployeeName { get; set; }

        public string SssNumber { get; set; }

        public DateTime LoanDate { get; set; }

        public decimal LoanAmount { get; set; }

        /// <summary>
        /// Total deducted for the month. Null for loans that stopped being paid.
        /// </summary>
        public decimal? TotalDeduction { get; set; }

        public string Remarks { get; set; }
    }
}
