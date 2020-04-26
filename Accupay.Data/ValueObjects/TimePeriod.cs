using System;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.ValueObjects
{
    public class TimePeriod
    {
        public readonly DateTime Start;

        public readonly DateTime End;

        public TimePeriod(DateTime start, DateTime end)
        {
            this.Start = start;
            this.End = end;
        }

        public static TimePeriod FromTime(TimeSpan from, TimeSpan to, DateTime date)
        {
            var dateFrom = date.Add(from);

            var nextDay = date.AddDays(1);
            var dateTo = to > from ? date.Add(to) : nextDay.Add(to);

            return new TimePeriod(dateFrom, dateTo);
        }

        public TimeSpan Length => End - Start;

        public decimal TotalHours => Convert.ToDecimal(Length.TotalHours);

        public decimal TotalMinutes => Convert.ToDecimal(Length.TotalMinutes);

        public bool Contains(DateTime moment) => (Start <= moment) && (moment <= End);

        public bool Intersects(TimePeriod period)
        {
            return this.Start <= period.End && this.End >= period.Start;
        }

        public bool EarlierThan(TimePeriod period) => this.Start <= period.Start;

        public bool LaterThan(TimePeriod period) => this.End >= period.End;

        public TimePeriod Overlap(TimePeriod period)
        {
            if (!Intersects(period))
                return null;

            return new TimePeriod(
                new DateTime[] { this.Start, period.Start }.Max(),
                new DateTime[] { this.End, period.End }.Min()
            );
        }

        public IList<TimePeriod> Difference(TimePeriod period)
        {
            if (!Intersects(period))
                return new List<TimePeriod>() { this };

            var periods = new List<TimePeriod>();
            if (Start < period.Start)
                periods.Add(new TimePeriod(Start, period.Start));

            if (End > period.End)
                periods.Add(new TimePeriod(period.End, End));

            return periods;
        }

        public override string ToString()
        {
            return $"{Start.ToString()} to {End.ToString()}";
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj is TimePeriod == false) return false;

            var compared = (TimePeriod)obj;

            return this.Start == compared.Start && this.End == compared.End;
        }

        public override int GetHashCode()
        {
            var hashCode = -418558944;
            hashCode = hashCode * -1521134295 + Start.GetHashCode();
            hashCode = hashCode * -1521134295 + End.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<TimeSpan>.Default.GetHashCode(Length);
            hashCode = hashCode * -1521134295 + TotalHours.GetHashCode();
            hashCode = hashCode * -1521134295 + TotalMinutes.GetHashCode();
            return hashCode;
        }
    }
}