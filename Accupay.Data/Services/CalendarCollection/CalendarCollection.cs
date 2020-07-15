using AccuPay.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Services
{
    public class CalendarCollection
    {
        private readonly bool _isUsingCalendars;

        private readonly IDictionary<int?, PayratesCalendar> _calendars;

        private readonly ICollection<Branch> _branches;

        private readonly PayratesCalendar _organizationCalendar;

        private readonly DefaultRates _defaultRates;

        // For identifying the _organizationCalendar
        public int OrganizationId { get; }

        public CalendarCollection(
            ICollection<PayRate> payrates,
            int organizationId,
            DefaultRates defaultRates)
        {
            _defaultRates = defaultRates;
            OrganizationId = organizationId;
            _isUsingCalendars = false;
            _organizationCalendar = new PayratesCalendar(payrates, _defaultRates);
        }

        public CalendarCollection(
            ICollection<PayRate> payrates,
            ICollection<Branch> branches,
            ICollection<CalendarDay> calendarDays,
            int organizationId,
            DefaultRates defaultRates)
            : this(payrates, organizationId, defaultRates)
        {
            _branches = branches;
            _calendars = calendarDays
                .GroupBy(t => t.CalendarID)
                .ToDictionary(
                    t => t.Key,
                    t => new PayratesCalendar(t, _defaultRates));
            _isUsingCalendars = true;
        }

        public PayratesCalendar GetCalendar(int? branchId = null)
        {
            var calendar = _isUsingCalendars ? FindCalendarByBranch(branchId) : _organizationCalendar;

            if (calendar == null)
                throw new Exception("No calendar was found");

            return calendar;
        }

        private PayratesCalendar FindCalendarByBranch(int? branchId)
        {
            if (branchId == null)
                return _organizationCalendar;

            var branch = _branches.FirstOrDefault(t => t.RowID == branchId);

            if (branch.CalendarID == null || !_calendars.ContainsKey(branch.CalendarID))
                return _organizationCalendar;

            return _calendars[branch.CalendarID];
        }
    }
}