using System;

namespace AccuPay.Web.TimeLogs
{
    public class UpdateEmployeeTimelogFilingDto
    {
        public string EmployeeNumber { get; set; }

        public string EntryType { get; set; }

        public DateTime LogDate { get; set; }

        public DateTime Time { get; set; }

        public string Reason { get; set; }

        public string DecidedBy { get; set; }
    }
}
