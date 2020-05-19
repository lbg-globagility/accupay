using System;

namespace AccuPay.Web.Allowances.Models
{
    public interface ICrudAllowanceDto
    {
        int ProductID { get; set; }

        string AllowanceFrequency { get; set; }

        DateTime? EffectiveEndDate { get; set; }

        DateTime EffectiveStartDate { get; set; }

        decimal Amount { get; set; }
    }
}
