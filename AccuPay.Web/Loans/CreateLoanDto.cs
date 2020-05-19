using System;

namespace AccuPay.Web.Loans
{
    public class CreateLoanDto : ICrudLoanDto
    {
        public int EmployeeId { get; set; }

        public int LoanTypeId { get; set; }

        public string LoanNumber { get; set; }

        public decimal TotalLoanAmount { get; set; }

        public DateTime EffectiveDate { get; set; }

        public decimal DeductionAmount { get; set; }

        public string Status { get; set; }

        public decimal DeductionPercentage { get; set; }

        public string DeductionSchedule { get; set; }

        public string Comments { get; set; }
    }
}
