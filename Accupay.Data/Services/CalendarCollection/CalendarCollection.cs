using AccuPay.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Services
{
    public class CalendarCollection
    {
        private readonly IDictionary<int?, PayratesCalendar> _calendars;

        private readonly ICollection<Branch> _branches;

        private readonly DefaultRates _defaultRates;

        private readonly PayratesCalendar _defaultCalendar;

        public CalendarCollection(
            ICollection<Branch> branches,
            ICollection<CalendarDay> calendarDays,
            DefaultRates defaultRates,
            PayCalendar defaultPayCalendar)
        {
            _branches = branches;
            _defaultRates = defaultRates;

            // Group the days based on the CalendarID into separate PayratesCalendar
            _calendars = calendarDays
                .GroupBy(t => t.CalendarID)
                .ToDictionary(
                    t => t.Key,
                    t => new PayratesCalendar(t, _defaultRates));

            // Create the default PayratesCalendar
            var defaultCalendarDays = calendarDays
                .Where(t => t.CalendarID == defaultPayCalendar.RowID);
            _defaultCalendar = new PayratesCalendar(defaultCalendarDays, defaultRates);
        }

        public PayratesCalendar GetCalendar(int? branchId = null)
        {
            var calendar = FindCalendarByBranch(branchId);

            if (calendar is null)
                throw new Exception("No calendar was found");

            return calendar;
        }

        private PayratesCalendar FindCalendarByBranch(int? branchId)
        {
            if (branchId is null)
                return _defaultCalendar;

            var branch = _branches.FirstOrDefault(t => t.RowID == branchId);

            if (branch.CalendarID is null || !_calendars.ContainsKey(branch.CalendarID))
                return _defaultCalendar;

            return _calendars[branch.CalendarID];
        }
    }
}