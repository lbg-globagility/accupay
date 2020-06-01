using System;

namespace AccuPay.Web.Payroll
{
    public class PayrollDto
    {
        public int? PayperiodId { get; set; }

        public DateTime CutoffStart { get; set; }

        public DateTime CutoffEnd { get; set; }

        public string Status { get; set; }
    }
}
