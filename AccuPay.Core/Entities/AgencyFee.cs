using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("agencyfee")]
    public class AgencyFee : EmployeeDataEntity
    {
        public int? AgencyID { get; set; }

        public int? DivisionID { get; set; }

        public int? TimeEntryID { get; set; }

        [Column("TimeEntryDate")]
        public DateTime Date { get; set; }

        [Column("DailyFee")]
        public decimal Amount { get; set; }
    }
}
