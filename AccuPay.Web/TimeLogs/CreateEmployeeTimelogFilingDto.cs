namespace AccuPay.Web.TimeLogs
{
    public class CreateEmployeeTimelogFilingDto : CrudEmployeeTimelogFilingDto
    {
        /// <summary>
        /// Optional. "Pending" (default) or "Approved". Send "Approved" for a filing that was
        /// already approved outside AccuPay (e.g. in Zoho People): its times are applied to the
        /// employee's time log right away.
        /// </summary>
        public string Status { get; set; }
    }
}
