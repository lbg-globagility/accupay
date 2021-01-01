using AccuPay.Core.Helpers;
using AccuPay.Core.Services.Policies;
using AccuPay.Utilities.Extensions;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("shiftschedules")]
    public class Shift : EmployeeDataEntity
    {
        [Column("Date")]
        public DateTime DateSched { get; set; }

        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public TimeSpan? BreakStartTime { get; set; }
        public decimal BreakLength { get; set; }
        public bool IsRestDay { get; set; }
        public decimal ShiftHours { get; internal set; }
        public decimal WorkHours { get; internal set; }

        [ForeignKey("EmployeeID")]
        public virtual Employee Employee { get; set; }

        [NotMapped]
        public DateTime? StartTimeFull
        {
            get => StartTime == null ?
                        (DateTime?)null :
                        DateSched.Date.ToMinimumHourValue().Add(StartTime.Value);

            set => StartTime = value == null ? null : value?.TimeOfDay;
        }

        [NotMapped]
        public DateTime? EndTimeFull
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
        ///

        public void ComputeShiftHours(ShiftBasedAutomaticOvertimePolicy shiftBasedAutomaticOvertimePolicy)
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

            RecomputeShiftHoursAndWorkHoursBaseOnPolicy(shiftBasedAutomaticOvertimePolicy);
        }

        private void ComputeWorkHours()
        {
            WorkHours = ShiftHours > BreakLength ? ShiftHours - BreakLength : 0;
        }

        private void RecomputeShiftHoursAndWorkHoursBaseOnPolicy(ShiftBasedAutomaticOvertimePolicy shiftBasedAutomaticOvertimePolicy)
        {
            if (StartTime.HasValue && EndTime.HasValue && shiftBasedAutomaticOvertimePolicy != null)
            {
                if (shiftBasedAutomaticOvertimePolicy.Enabled && WorkHours > shiftBasedAutomaticOvertimePolicy.DefaultWorkHours)
                {
                    WorkHours = shiftBasedAutomaticOvertimePolicy.DefaultWorkHours;
                    ShiftHours = WorkHours + BreakLength;
                }
            }
        }
    }
}
