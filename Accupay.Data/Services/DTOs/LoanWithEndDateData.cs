using AccuPay.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Services.DTOs
{
    public class LoanWithEndDateData
    {
        public LoanWithEndDateData(Loan loan, ICollection<PayPeriod> payPeriods)
        {
            Id = loan.RowID.Value;
            DedEffectiveDateFrom = loan.DedEffectiveDateFrom;
            DeductionAmount = loan.DeductionAmount;
            TotalBalanceLeft = loan.TotalBalanceLeft;
            IsCompleted = loan.Status == Loan.STATUS_COMPLETE;
            DedEffectiveDateTo = payPeriods.Max(pp => pp.PayToDate);
        }

        public int Id { get; }
        public DateTime DedEffectiveDateFrom { get; }
        public decimal DeductionAmount { get; }
        public decimal TotalBalanceLeft { get; }
        public bool IsCompleted { get; }
        public DateTime DedEffectiveDateTo { get; }
    }
}
