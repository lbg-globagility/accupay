using System;

namespace AccuPay.Web.Leaves
{
    public class LeaveFilter
    {
        public string Term { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }
    }
}
