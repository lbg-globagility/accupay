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
using AccuPay.Utilities.Extensions;

namespace AccuPay.Data.Entities
{
    [Table("employeeovertime")]
    public class Overtime
    {
        public const string StatusApproved = "Approved";

        public const string StatusPending = "Pending";

        public const string DefaultType = "Overtime";

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

        [Column("OTType")]
        public string Type { get; set; }

        public TimeSpan? OTStartTime { get; set; }

        public TimeSpan? OTEndTime { get; set; }

        public DateTime OTStartDate { get; set; }

        public DateTime OTEndDate { get; set; }

        [Column("OTStatus")]
        public string Status { get; set; }

        [NotMapped]
        public DateTime? Start { get; set; }

        [NotMapped]
        public DateTime? End { get; set; }

        public string Reason { get; set; }

        public string Comments { get; set; }

        public Overtime()
        {
            Status = StatusPending;
        }

        [NotMapped]
        public DateTime? OTStartTimeFull
        {
            get
            {
                // Using Nothing as output on ternary operator does not work
                if (OTStartTime == null)
                    return default(DateTime?);
                else
                    return OTStartDate.Date.ToMinimumHourValue().Add(OTStartTime.Value);
            }
            set
            {
                // Using Nothing as output on ternary operator does not work
                if (value == null)
                    OTStartTime = default(TimeSpan?);
                else
                    OTStartTime = value?.TimeOfDay;
            }
        }

        [NotMapped]
        public DateTime? OTEndTimeFull
        {
            get
            {
                // Using Nothing as output on ternary operator does not work
                if (OTEndTime == null)
                    return default(DateTime?);
                else
                    return OTEndDate.Date.ToMinimumHourValue().Add(OTEndTime.Value);
            }
            set
            {
                // Using Nothing as output on ternary operator does not work
                if (value == null)
                    OTEndTime = default(TimeSpan?);
                else
                    OTEndTime = value?.TimeOfDay;
            }
        }

        public string Validate()
        {
            if (OTStartTime == null)
                return "Start Time cannot be empty.";

            if (OTEndTime == null)
                return "End Time cannot be empty.";

            if (OTStartDate.Date.Add(OTStartTime.Value.StripSeconds()) >= OTEndDate.Date.Add(OTEndTime.Value.StripSeconds()))
                return "Start date and time cannot be greater than or equal to End date and time.";

            string[] invalidStatuses = { StatusPending, StatusApproved };
            if (!invalidStatuses.Contains(Status))
            {
                return "Status is not valid.";
            }

            // Means no error
            return null;
        }
    }
}