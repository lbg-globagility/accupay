using System;

namespace AccuPay.Web.OfficialBusinesses
{
    public class SelfServiceCreateOfficialBusinessDto
    {
        public DateTime Date { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string Reason { get; set; }
    }
}
