using AccuPay.Core.Entities;
using AccuPay.Core.ValueObjects;
using AccuPay.Utilities.Extensions;
using System;
using System.Linq;

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

                var breakTimeEnd = shift.BreakStartTime.Value.AddHours(shift.BreakLength);
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

        private const int HOURS_PER_DAY = 24;

        internal bool IsValidLogPeriod(TimePeriod logPeriod)
        {
            var nonShiftHours = HOURS_PER_DAY - ShiftHours;
            var splittedNonShiftHours = nonShiftHours / 2;
            var veryStartTime = Start.AddHours(-(double)splittedNonShiftHours);

            var timeCollection = Enumerable.Range(0, HOURS_PER_DAY)
                .Select(t =>
                {
                    var fsdfsd = veryStartTime.AddHours(t);

                    return new
                    {
                        startRange = fsdfsd,
                        endRange = fsdfsd.AddHours((double)ShiftHours)
                    };
                })
                .ToList();

            var fsdf = timeCollection
                .Where(t => t.startRange <= logPeriod.Start)
                .Where(t => t.endRange >= logPeriod.End)
                .ToList();

            return fsdf?.Any() ?? false;
        }

        public bool MarkedAsWholeDay => _shift?.MarkedAsWholeDay ?? false;

        public int? GracePeriod => _shift?.GracePeriod;
    }
}
