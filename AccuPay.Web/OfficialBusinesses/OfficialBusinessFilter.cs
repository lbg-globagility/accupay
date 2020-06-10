using System;

namespace AccuPay.Web.OfficialBusinesses
{
    public class OfficialBusinessFilter
    {
        public string Term { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }
    }
}
