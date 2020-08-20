using System;

namespace AccuPay.Data.Services.Imports.Overtimes
{
    public class OvertimeModel
    {
        public int? EmployeeID { get; set; }
        public string EmployeeNo { get; set; }
        public string FullName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}