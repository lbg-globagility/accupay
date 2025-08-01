using System;
using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Loans
{
    public class LoanDto
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public string EmployeeNumber { get; set; }

        public string EmployeeName { get; set; }

        public string EmployeeType { get; set; }

        public int LoanTypeId { get; set; }

        public string LoanType { get; set; }

        [Required]
        public string LoanNumber { get; set; }

        public decimal TotalLoanAmount { get; set; }

        public decimal TotalBalanceLeft { get; set; }

        public DateTime StartDate { get; set; }

        public int LoanPayPeriodLeft { get; set; }

        public decimal DeductionAmount { get; set; }

        public string Status { get; set; }

        public decimal DeductionPercentage { get; set; }

        public string DeductionSchedule { get; set; }

        public string Comments { get; set; }
    }
}
