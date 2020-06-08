using System;
using System.Collections.Generic;
using System.Text;

namespace AccuPay.Web.Leaves
{
    public class LeaveLedgerDto
    {
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        public string EmployeeNumber { get; set; }

        public string EmployeeName { get; set; }

        public string EmployeeType { get; set; }

        public string TransactionType { get; set; }

        public DateTime Date { get; set; }

        public decimal? Balance { get; set; }

        public decimal? Amount { get; set; }
    }
}
