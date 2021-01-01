using AccuPay.Core.Entities;
using AccuPay.Core.ValueObjects;
using AccuPay.Utilities.Extensions;
using System;

namespace AccuPay.Core.Helpers
{
    public class CurrentShift
    {
        public const decimal StandardWorkingHours = 8;

        private int? _defaultRestDay;
        private readonly Shift _shift;

        public CurrentShift(Shift shift, DateTime date)
        {
            this.Date = date;

            if (shift == null || shift.StartTime == null || shift.EndTime == null)
                return;

            _shift = shift;

            this.ShiftPeriod = TimePeriod.FromTime(
                new TimeSpan(shift.StartTime.Value.Hours, shift.StartTime.Value.Minutes, 0),
                new TimeSpan(shift.EndTime.Value.Hours, shift.EndTime.Value.Minutes, 0),
                this.Date);

            if (shift.BreakStartTime.HasValue)
            {
                var nextDay = this.Date.AddDays(1);
                var breakDate = shift.BreakStartTime > shift.StartTime ? this.Date : nextDay;

                var breakTimeEnd = shift.BreakStartTime.Value.AddHours(Convert.ToInt32(shift.BreakLength));
                this.BreakPeriod = TimePeriod.FromTime(shift.BreakStartTime.Value, breakTimeEnd, breakDate);
            }
        }

        public DateTime Start => ShiftPeriod.Start;

        public DateTime End => ShiftPeriod.End;

        public DateTime? BreaktimeStart => BreakPeriod.Start;

        public DateTime? BreaktimeEnd => BreakPeriod.End;

        public DateTime Date { get; }

        public TimePeriod ShiftPeriod { get; }

        public TimePeriod BreakPeriod { get; }

        public decimal WorkingHours => _shift?.WorkHours ?? StandardWorkingHours;

        public decimal ShiftHours => _shift?.ShiftHours ?? StandardWorkingHours + 1;

        public bool HasShift => _shift != null;

        public bool HasBreaktime => BreakPeriod != null;

        public bool IsRestDay
        {
            get
            {
                var isRestDayOffset = false;

                if (_shift != null)
                    isRestDayOffset = _shift.IsRestDay;

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

        public TimeSpan? StartTime => _shift?.StartTime;

        public TimeSpan? EndTime => _shift?.EndTime;

        public void SetDefaultRestDay(int? dayOfWeek) => _defaultRestDay = dayOfWeek;

        public override string ToString()
        {
            return $"{Start:yyyy-MM-dd hh:mm tt} - {End:yyyy-MM-dd hh:mm tt} | {BreaktimeStart?.ToString("yyyy-MM-dd hh:mm tt")} - {BreaktimeEnd?.ToString("yyyy-MM-dd hh:mm tt")} ";
        }
    }
}