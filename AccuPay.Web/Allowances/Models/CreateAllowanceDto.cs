using System;

namespace AccuPay.Web.Allowances.Models
{
    public class CreateAllowanceDto : ICrudAllowanceDto
    {
        public int EmployeeId { get; set; }

        public int AllowanceTypeId { get; set; }

        public string Frequency { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public decimal Amount { get; set; }
    }
}
