using System;

namespace AccuPay.Web.TimeLogs
{
    public class CreateEmployeeTimelogFilingDto
    {
        public string EmployeeNumber { get; set; }

        public string EntryType { get; set; }

        public DateTime LogDate { get; set; }

        public DateTime Time { get; set; }

        public string Reason { get; set; }

        /// <summary>
        /// Optional. "Pending" (default) or "Approved". Send "Approved" for a filing that was
        /// already approved outside AccuPay (e.g. in Zoho People): the time is applied to the
        /// employee's time log right away. An approved filing's EntryType must be CheckIn,
        /// CheckOut, LunchOut or LunchIn.
        /// </summary>
        public string Status { get; set; }
    }
}
