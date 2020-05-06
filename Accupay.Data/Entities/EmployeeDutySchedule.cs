using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("shiftschedules")]
    public class EmployeeDutySchedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RowID { get; set; }

        public int? OrganizationID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime LastUpd { get; set; }

        public int? LastUpdBy { get; set; }

        [ForeignKey("Employee")]
        public int? EmployeeID { get; set; }

        [Column("Date")]
        public DateTime DateSched { get; set; }

        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public TimeSpan? BreakStartTime { get; set; }
        public decimal BreakLength { get; set; }
        public bool IsRestDay { get; set; }
        public decimal ShiftHours { get; set; }
        public decimal WorkHours { get; set; }

        public virtual Employee Employee { get; set; }
    }
}