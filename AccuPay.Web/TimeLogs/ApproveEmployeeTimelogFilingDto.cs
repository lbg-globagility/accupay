using System;
using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.TimeLogs
{
    public class ApproveEmployeeTimelogFilingDto
    {
        [Required]
        public string EmployeeNumber { get; set; }

        public DateTime LogDate { get; set; }

        public string DecidedBy { get; set; }
    }
}
