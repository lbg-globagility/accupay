using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("jobpoint")]
    public class JobPoint
    {
        public int? RowID { get; set; }

        public int? OrganizationID { get; set; }

        public int? EmployeeID { get; set; }

        public DateTime OccurredOn { get; set; }

        public int Points { get; set; }

        public string Comments { get; set; }

        [ForeignKey("EmployeeID")]
        public virtual Employee Employee { get; set; }
    }
}