using System;

namespace AccuPay.Web.Loans
{
    public interface ICrudLoanDto
    {
        int LoanTypeId { get; set; }

        string LoanNumber { get; set; }

        decimal TotalLoanAmount { get; set; }

        DateTime EffectiveDate { get; set; }

        decimal DeductionAmount { get; set; }

        string Status { get; set; }

        decimal DeductionPercentage { get; set; }

        string DeductionSchedule { get; set; }

        string Comments { get; set; }
    }
}
