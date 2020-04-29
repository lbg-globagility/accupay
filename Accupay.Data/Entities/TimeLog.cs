using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("employeetimeentrydetails")]
    public class TimeLog
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

        public int? EmployeeID { get; set; }

        [Column("Date")]
        public DateTime LogDate { get; set; }

        public TimeSpan? TimeIn { get; set; }

        public TimeSpan? TimeOut { get; set; }

        public DateTime? TimeStampIn { get; set; }

        public DateTime? TimeStampOut { get; set; }

        public string TimeentrylogsImportID { get; set; }
        public int? BranchID { get; set; }

        public TimeLog()
        {
        }

        public TimeLog(string timeIn, string timeOut)
        {
            this.TimeIn = TimeSpan.Parse(timeIn);
            this.TimeOut = TimeSpan.Parse(timeOut);
        }

        [ForeignKey("EmployeeID")]
        public virtual Employee Employee { get; set; }

        public bool HasLogs => TimeIn.HasValue && TimeOut.HasValue;
    }
}