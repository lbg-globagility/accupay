using System;

namespace AccuPay.Web.Allowances.Models
{
    public class CreateAllowanceDto : ICrudAllowanceDto
    {
        public int EmployeeID { get; set; }

        public int ProductID { get; set; }

        public string AllowanceFrequency { get; set; }

        public DateTime? EffectiveEndDate { get; set; }

        public DateTime EffectiveStartDate { get; set; }

        public decimal Amount { get; set; }
    }
}
