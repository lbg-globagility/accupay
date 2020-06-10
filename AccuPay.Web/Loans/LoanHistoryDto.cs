using System;

namespace AccuPay.Web.Loans
{
    public class LoanHistoryDto
    {
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        public string EmployeeNo { get; set; }

        public string EmployeeName { get; set; }

        public string EmployeeType { get; set; }

        public DateTime DeductionDate { get; set; }

        public decimal Amount { get; set; }

        public decimal Balance { get; set; }
    }
}
