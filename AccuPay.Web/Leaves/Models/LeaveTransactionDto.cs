using System;

namespace AccuPay.Web.Leaves
{
    public class LeaveTransactionDto
    {
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        public string EmployeeNo { get; set; }

        public string EmployeeName { get; set; }

        public string EmployeeType { get; set; }

        public string Description { get; set; }

        public string TransactionType { get; set; }

        public DateTime Date { get; set; }

        public decimal? Balance { get; set; }

        public decimal? Amount { get; set; }
    }
}
