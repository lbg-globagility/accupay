using System;

namespace AccuPay.Web.Overtimes
{
    public class OvertimeFilter
    {
        public string Term { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }
    }
}
