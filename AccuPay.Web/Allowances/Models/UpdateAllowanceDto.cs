using System;

namespace AccuPay.Web.Allowances.Models
{
    public class UpdateAllowanceDto : ICrudAllowanceDto
    {
        public int ProductID { get; set; }

        public string AllowanceFrequency { get; set; }

        public DateTime? EffectiveEndDate { get; set; }

        public DateTime EffectiveStartDate { get; set; }

        public decimal Amount { get; set; }
    }
}
