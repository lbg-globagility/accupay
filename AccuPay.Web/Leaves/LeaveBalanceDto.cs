using System;

namespace AccuPay.Web.Leaves
{
    public class LeaveBalanceDto
    {
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        public string EmployeeNumber { get; set; }

        public string EmployeeName { get; set; }

        public string EmployeeType { get; set; }

        public decimal? VacationLeave { get; set; }

        public decimal? SickLeave { get; set; }
    }
}
