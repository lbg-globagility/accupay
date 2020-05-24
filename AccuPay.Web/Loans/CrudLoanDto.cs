using System;
using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Loans
{
    public abstract class CrudLoanDto
    {
        private const double MinimumAmount = 0.01;
        private const double MaximumAmount = 99999999.99;

        [Required]
        [Range(MinimumAmount, MaximumAmount)]
        public decimal TotalLoanAmount { get; set; }

        [Required]
        [Range(0, 99)]
        public decimal DeductionPercentage { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        [Range(MinimumAmount, MaximumAmount)]
        public decimal DeductionAmount { get; set; }

        public string LoanNumber { get; set; }

        [Required]
        public int LoanTypeId { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public string DeductionSchedule { get; set; }

        public string Comments { get; set; }
    }
}
