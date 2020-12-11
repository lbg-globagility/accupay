using AccuPay.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Services.DTOs
{
    public class LoanScheduleWithEndDateData
    {
        public LoanScheduleWithEndDateData(LoanSchedule loanSchedule, ICollection<PayPeriod> payPeriods)
        {
            Id = loanSchedule.RowID.Value;
            DedEffectiveDateFrom = loanSchedule.DedEffectiveDateFrom;
            DeductionAmount = loanSchedule.DeductionAmount;
            TotalBalanceLeft = loanSchedule.TotalBalanceLeft;
            IsCompleted = loanSchedule.Status == LoanSchedule.STATUS_COMPLETE;
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
