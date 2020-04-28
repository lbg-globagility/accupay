using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("employeeshift")]
    public class ShiftSchedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? RowID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpd { get; set; }

        public int? LastUpdBy { get; set; }

        public int OrganizationID { get; set; }

        public int? EmployeeID { get; set; }

        public int? ShiftID { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime EffectiveTo { get; set; }

        [Column("NightShift")]
        public bool IsNightShift { get; set; }

        [Column("RestDay")]
        public bool IsRestDay { get; set; }

        public bool IsWorkingDay => !IsRestDay;

        [ForeignKey("ShiftID")]
        public virtual Shift Shift { get; set; }
    }
}