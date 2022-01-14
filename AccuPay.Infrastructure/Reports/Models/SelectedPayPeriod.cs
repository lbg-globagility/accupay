using AccuPay.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccuPay.Infrastructure.Reports.Models
{
    public class SelectedPayPeriod
    {
        public PayPeriod From { get; set; }
        public PayPeriod To { get; set; }

        public int FromId => From.RowID.Value;
        public int ToId => To.RowID.Value;

        public DateTime DateFrom => From.PayFromDate;
        public DateTime DateTo => To.PayToDate;

        public SelectedPayPeriod(PayPeriod from, PayPeriod to)
        {
            From = from;
            To = to;
        }
    }
}
