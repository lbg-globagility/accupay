using AccuPay.Core.Entities;
using System;

namespace AccuPay.Core.UnitTests.Mothers
{
    public class PayPeriodMother
    {
        public static PayPeriod StartDateOnly(DateTime payFromDate)
        {
            PayPeriod payPeriod = new PayPeriod()
            {
                PayFromDate = payFromDate
            };

            return payPeriod;
        }
    }
}
