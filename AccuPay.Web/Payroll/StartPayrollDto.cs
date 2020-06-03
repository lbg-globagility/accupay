using System;
using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Payroll
{
    public class StartPayrollDto
    {
        [Required]
        public DateTime CutoffStart { get; set; }

        [Required]
        public DateTime CutoffEnd { get; set; }
    }
}
