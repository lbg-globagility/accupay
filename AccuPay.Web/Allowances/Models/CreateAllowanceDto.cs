using System;

namespace AccuPay.Web.Allowances.Models
{
    public class CreateAllowanceDto : ICrudAllowanceDto
    {
        public int EmployeeId { get; set; }

        public int ProductId { get; set; }

        public string AllowanceFrequency { get; set; }

        public DateTime? EffectiveEndDate { get; set; }

        public DateTime EffectiveStartDate { get; set; }

        public decimal Amount { get; set; }
    }
}
