using System.Collections.Generic;

namespace AccuPay.Web.Payroll
{
    public class PayrollResultDto
    {
        public int Successes { get; set; }

        public int Errors { get; set; }

        public List<PayrollResultDetailsDto> Details { get; set; }
    }
}
