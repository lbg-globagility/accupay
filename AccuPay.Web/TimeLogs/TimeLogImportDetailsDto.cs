using System;

namespace AccuPay.Web.TimeLogs
{
    public class TimeLogImportDetailsDto
    {
        public string EmployeeNumber { get; set; }

        public string EmployeeName { get; set; }

        public DateTime DateAndTime { get; set; }

        public string ErrorMessage { get; set; }

        public string LineContent { get; set; }

        public int LineNumber { get; set; }

        public string Type { get; set; }
    }
}
