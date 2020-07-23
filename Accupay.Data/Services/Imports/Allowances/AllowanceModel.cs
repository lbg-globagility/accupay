using System;
using System.Collections.Generic;
using System.Text;

namespace AccuPay.Data.Services.Imports.Allowances
{
    public class AllowanceModel
    {
        public int? EmployeeId { get; set; }
        public string EmployeeNo { get; set; }
        public int? ProductId { get; set; }
        public int? AllowanceTypeId { get; set; }
        public string AllowanceName { get; internal set; }
        public DateTime? EffectiveStartDate { get; set; }
        public string AllowanceFrequency { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public decimal? Amount { get; set; }
    }
}