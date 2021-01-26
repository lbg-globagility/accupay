using System;
using System.Collections.Generic;
using System.Text;

namespace AccuPay.Core.Services.Imports.OfficialBusiness
{
    public class OfficialBusinessModel
    {
        public int? EmployeeID { get; set; }
        public string EmployeeNo { get; set; }
        public string FullName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}