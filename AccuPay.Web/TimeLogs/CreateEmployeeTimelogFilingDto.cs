using System;

namespace AccuPay.Web.TimeLogs
{
    public class CreateEmployeeTimelogFilingDto
    {
        public int EmployeeId { get; set; }

        public string EntryType { get; set; }

        public DateTime LogDate { get; set; }

        public DateTime Time { get; set; }

        public string Reason { get; set; }
    }
}
