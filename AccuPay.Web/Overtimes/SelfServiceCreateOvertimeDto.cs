using System;
using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Overtimes
{
    public class SelfServiceCreateOvertimeDto
    {
        [Required]
        public string EmployeeNumber { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        public string Reason { get; set; }

        /// <summary>
        /// Optional. "Pending" (default) or "Approved". Send "Approved" for an overtime that was
        /// already approved outside AccuPay (e.g. in Zoho People) to save it as approved.
        /// </summary>
        public string Status { get; set; }
    }
}
