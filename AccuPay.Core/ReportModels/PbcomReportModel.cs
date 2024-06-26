using System;
using System.Collections.Generic;
using System.Text;

namespace AccuPay.Core.ReportModels
{
    public class PbcomReportModel
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public decimal TotalNetSalary { get; set; }
        public string ATMNo { get; set; }
    }
}
