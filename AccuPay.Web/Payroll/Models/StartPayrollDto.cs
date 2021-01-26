using System;
using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Payroll
{
    public class StartPayrollDto
    {
        [Required]
        public int Month { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public bool IsFirstHalf { get; set; }
    }
}
