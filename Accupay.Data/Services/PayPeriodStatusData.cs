using AccuPay.Data.Entities;
using AccuPay.Data.Enums;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Services
{
    public class PayPeriodStatusData
    {
        private readonly PayrollContext context;

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

        public PayPeriodStatusData(PayrollContext context)
        {
            this.context = context;
        }
    }
}