using System;
using System.Collections.Generic;
using System.Text;

namespace AccuPay.Data.Services.Imports.Loans
{
    public class LoanModel
    {
        public string EmployeeNo { get; set; }

        public int EmployeeId { get; set; }

        public string LoanName { get; set; }

        public int LoanTypeId { get; set; }

        public string LoanNumber { get; set; }

        public DateTime StartDate { get; set; }

        public decimal TotalLoanAmount { get; set; }

        public decimal TotalLoanBalance { get; set; }

        public decimal DeductionAmount { get; set; }

        public string DeductionSchedule { get; set; }

        public string Comments { get; set; }

        public string Status { get; set; }
    }
}