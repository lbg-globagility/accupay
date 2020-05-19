using System;

namespace AccuPay.Web.Allowances.Models
{
    public interface ICrudAllowanceDto
    {
        int AllowanceTypeId { get; set; }

        string Frequency { get; set; }

        DateTime StartDate { get; set; }

        DateTime? EndDate { get; set; }

        decimal Amount { get; set; }
    }
}
