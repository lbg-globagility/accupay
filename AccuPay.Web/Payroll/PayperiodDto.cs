using AccuPay.Data.Entities;
using System;

namespace AccuPay.Web.Payroll
{
    public class PayperiodDto
    {
        public int? Id { get; set; }

        public DateTime CutoffStart { get; set; }

        public DateTime CutoffEnd { get; set; }

        public string Status { get; set; }

        public void ApplyData(PayPeriod payPeriod)
        {
            Id = payPeriod.RowID;
            CutoffStart = payPeriod.PayFromDate;
            CutoffEnd = payPeriod.PayToDate;
            Status = payPeriod.Status.ToString();
        }
    }
}
