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

        public CurrentShift(Shift shift, DateTime date)
        {
            this.Shift = shift;
            this.Date = date;

            if (shift == null) return;

            this.ShiftPeriod = TimePeriod.FromTime(new TimeSpan(shift.TimeFrom.Hours,
                                                                shift.TimeFrom.Minutes, 0),
                                                    new TimeSpan(shift.TimeTo.Hours,
                                                                shift.TimeTo.Minutes, 0),
                                                    date);

            if (shift.HasBreaktime)
            {
                var nextDay = date.AddDays(1);
                var breakDate = shift.BreaktimeFrom > shift.TimeFrom ? date : nextDay;

                this.BreakPeriod = TimePeriod.FromTime(shift.BreaktimeFrom.Value,
                                                        shift.BreaktimeTo.Value,
                                                        breakDate);
            }
        }

        public CurrentShift(ShiftSchedule shiftSchedule, DateTime date) : this(shiftSchedule?.Shift, date)
        {
            this.ShiftSchedule = shiftSchedule;
        }

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

        public Shift Shift { get; }

        public TimePeriod ShiftPeriod { get; }

        public TimePeriod BreakPeriod { get; }

        public ShiftSchedule ShiftSchedule { get; }

        public decimal WorkingHours => _shiftSchedule2?.WorkHours ??
                                            ShiftSchedule?.Shift?.WorkHours ??
                                                StandardWorkingHours;

        public decimal ShiftHours => _shiftSchedule2?.ShiftHours ??
                                            ShiftSchedule?.Shift?.ShiftHours ??
                                                StandardWorkingHours + 1;

        public bool HasShift => Shift != null || _shiftSchedule2 != null;

        public bool HasBreaktime => BreakPeriod != null;

        public bool IsRestDay
        {
            get
            {
                var isRestDayOffset = false;

                if (_shiftSchedule2 != null)
                    isRestDayOffset = _shiftSchedule2.IsRestDay;
                else if (ShiftSchedule != null)
                    isRestDayOffset = ShiftSchedule.IsRestDay;

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

        public TimeSpan? StartTime => _shiftSchedule2?.StartTime ?? ShiftSchedule?.Shift?.TimeFrom;

        public TimeSpan? EndTime => _shiftSchedule2?.EndTime ?? ShiftSchedule?.Shift?.TimeTo;

        public void SetDefaultRestDay(int? dayOfWeek) => _defaultRestDay = dayOfWeek;

        public override string ToString()
        {
            return $"{Start.ToString("yyyy-MM-dd hh:mm tt")} - {End.ToString("yyyy-MM-dd hh:mm tt")} | {BreaktimeStart?.ToString("yyyy-MM-dd hh:mm tt")} - {BreaktimeEnd?.ToString("yyyy-MM-dd hh:mm tt")} ";
        }
    }
}