using AccuPay.Data.Entities;
using AccuPay.Data.ValueObjects;
using AccuPay.Utilities.Extensions;
using System;

namespace AccuPay.Data.Helpers
{
    public class CurrentShift
    {
        public const decimal StandardWorkingHours = 8;

        private int? _defaultRestDay;

        private EmployeeDutySchedule _shiftSchedule2;

        public CurrentShift(EmployeeDutySchedule shiftSchedule, DateTime date)
        {
            this.Date = date;

            if (shiftSchedule == null || shiftSchedule.StartTime == null || shiftSchedule.EndTime == null)
                return;

            _shiftSchedule2 = shiftSchedule;

            this.ShiftPeriod = TimePeriod.FromTime(new TimeSpan(shiftSchedule.StartTime.Value.Hours, shiftSchedule.StartTime.Value.Minutes, 0), new TimeSpan(shiftSchedule.EndTime.Value.Hours, shiftSchedule.EndTime.Value.Minutes, 0), this.Date);

            if (shiftSchedule.BreakStartTime.HasValue)
            {
                var nextDay = this.Date.AddDays(1);
                var breakDate = shiftSchedule.BreakStartTime > shiftSchedule.StartTime ? this.Date : nextDay;

                var breakTimeEnd = shiftSchedule.BreakStartTime.Value.AddHours(Convert.ToInt32(shiftSchedule.BreakLength));
                this.BreakPeriod = TimePeriod.FromTime(shiftSchedule.BreakStartTime.Value, breakTimeEnd, breakDate);
            }
        }

        public DateTime Start => ShiftPeriod.Start;

        public DateTime End => ShiftPeriod.End;

        public DateTime? BreaktimeStart => BreakPeriod.Start;

        public DateTime? BreaktimeEnd => BreakPeriod.End;

        public DateTime Date { get; }

        public TimePeriod ShiftPeriod { get; }

        public TimePeriod BreakPeriod { get; }

        public decimal WorkingHours => _shiftSchedule2?.WorkHours ?? StandardWorkingHours;

        public decimal ShiftHours => _shiftSchedule2?.ShiftHours ?? StandardWorkingHours + 1;

        public bool HasShift => _shiftSchedule2 != null;

        public bool HasBreaktime => BreakPeriod != null;

        public bool IsRestDay
        {
            get
            {
                var isRestDayOffset = false;

                if (_shiftSchedule2 != null)
                    isRestDayOffset = _shiftSchedule2.IsRestDay;

                var isDefaultRestDay = false;
                if (_defaultRestDay.HasValue)
                {
                    isDefaultRestDay = (_defaultRestDay.Value - 1) == (int)this.Date.DayOfWeek;
                }

                return (isRestDayOffset && (!isDefaultRestDay)) ||
                        ((!isRestDayOffset) && isDefaultRestDay);
            }
        }

        public bool IsWorkingDay => !IsRestDay;

        public bool IsNightShift => true;

        public TimeSpan? StartTime => _shiftSchedule2?.StartTime;

        public TimeSpan? EndTime => _shiftSchedule2?.EndTime;

        public void SetDefaultRestDay(int? dayOfWeek) => _defaultRestDay = dayOfWeek;

        public override string ToString()
        {
            return $"{Start:yyyy-MM-dd hh:mm tt} - {End:yyyy-MM-dd hh:mm tt} | {BreaktimeStart?.ToString("yyyy-MM-dd hh:mm tt")} - {BreaktimeEnd?.ToString("yyyy-MM-dd hh:mm tt")} ";
        }
    }
}