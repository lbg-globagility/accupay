using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("employeetimeattendancelog")]
    public class TimeAttendanceLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? RowID { get; set; }

        public int? OrganizationID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpd { get; set; }

        public int? LastUpdBy { get; set; }

        public string ImportNumber { get; set; }

        public DateTime TimeStamp { get; set; }

        public bool? IsTimeIn { get; set; }

        public DateTime WorkDay { get; set; }

        public int EmployeeID { get; set; }

        [ForeignKey("EmployeeID")]
        public virtual Employee Employee { get; set; }

        public string IsTimeInDescription
        {
            get
            {
                if (IsTimeIn == null)
                    return "";

                return IsTimeIn == true ? "IN" : "OUT";
            }
        }
    }
}