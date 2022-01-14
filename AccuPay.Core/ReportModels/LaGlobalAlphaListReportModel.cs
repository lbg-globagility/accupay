using System;

namespace AccuPay.Core.ReportModels
{
    public class LaGlobalAlphaListReportModel
    {
        public string EmployeeId { get; set; }

        public string Name { get; set; }

        public string TIN { get; set; }

        public string Address { get; set; }

        public string BirthDate { get; set; }

        public string ContactNo { get; set; }

        public decimal Gross { get; set; }

        public decimal SSS { get; set; }

        public decimal PhilHealth { get; set; }

        public decimal HDMF { get; set; }

        public decimal ThirteenthMonthPay { get; set; }
    }
}
