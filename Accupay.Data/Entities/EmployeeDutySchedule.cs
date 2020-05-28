using AccuPay.Data.Helpers;
using AccuPay.Utilities.Extensions;
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
        public decimal ShiftHours { get; internal set; }
        public decimal WorkHours { get; internal set; }

        public virtual Employee Employee { get; set; }

        [NotMapped]
        public DateTime? ShiftStartTimeFull
        {
            get => StartTime == null ?
                        (DateTime?)null :
                        DateSched.Date.ToMinimumHourValue().Add(StartTime.Value);

            set => StartTime = value == null ? null : value?.TimeOfDay;
        }

        [NotMapped]
        public DateTime? ShiftEndTimeFull
        {
            get => EndTime == null ?
                        (DateTime?)null :
                        DateSched.Date.ToMinimumHourValue().Add(EndTime.Value);

            set => EndTime = value == null ? null : value?.TimeOfDay;
        }

        [NotMapped]
        public DateTime? ShiftBreakStartTimeFull
        {
            get => BreakStartTime == null ?
                        (DateTime?)null :
                        DateSched.Date.ToMinimumHourValue().Add(BreakStartTime.Value);

            set => BreakStartTime = value == null ? null : value?.TimeOfDay;
        }

        /// <summary>
        /// Computes the shift hours and also update the work hours.
        /// </summary>
        public void ComputeShiftHours()
        {
            if (StartTime.HasValue && EndTime.HasValue)
            {
                var trueEndTime = StartTime > EndTime ? EndTime.Value.AddOneDay() : EndTime.Value;

                double totalMinutes = (trueEndTime - StartTime).Value.TotalMinutes;

                ShiftHours = Convert.ToDecimal(totalMinutes / TimeConstants.MinutesPerHour);
            }
            else
            {
                ShiftHours = 0;
            }

            ComputeWorkHours();
        }

        private void ComputeWorkHours() => WorkHours = ShiftHours > BreakLength ? ShiftHours - BreakLength : 0;
    }
}