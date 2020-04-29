using AccuPay.Data.Entities;
using AccuPay.Data.Enums;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Services
{
    public class PayPeriodStatusData
    {
        public enum PayPeriodStatus
        {
            Open,
            Processing,
            Closed
        }

        public class PayPeriodStatusReference
        {
            public PayPeriodStatus PayPeriodStatus { get; set; }

            public PayPeriodStatusReference(PayPeriodStatus payPeriodStatus)
            {
                this.PayPeriodStatus = payPeriodStatus;
            }
        }

        public int Index { get; set; }
        public PayPeriodStatus Status { get; set; }

        public static List<PayPeriod> GetPeriodsWithPaystubCount(int organizationId, string payFreqType = "SEMI-MONTHLY")
        {
            using (PayrollContext context = new PayrollContext())
            {
                var payFrequencyId = payFreqType == "WEEKLY" ? PayFrequencyType.Weekly : PayFrequencyType.SemiMonthly;

                return context.PayPeriods.
                        Where(p => p.OrganizationID == organizationId).
                        Where(p => p.PayFrequencyID == (int)payFrequencyId).
                        Where(p => !p.IsClosed).
                        Where(p => context.Paystubs.
                                    Where(s => s.PayPeriodID == p.RowID.Value).
                                    Any()).
                        ToList();
            }
        }
    }
}