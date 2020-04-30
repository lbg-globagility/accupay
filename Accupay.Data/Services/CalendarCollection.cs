using AccuPay.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Services
{
    public class CalendarCollection
    {
        private bool _isUsingCalendars;

        private ICollection<Branch> _branches;

        private IDictionary<int?, PayratesCalendar> _calendars;

        private PayratesCalendar _organizationCalendar;

        // for identifying the _organizationCalendar
        public int OrganizationId { get; }

        public CalendarCollection(ICollection<PayRate> payrates, int organizationId)
        {
            _organizationCalendar = new PayratesCalendar(payrates);
            OrganizationId = organizationId;
            _isUsingCalendars = false;
        }

        public CalendarCollection(ICollection<PayRate> payrates, ICollection<Branch> branches, ICollection<CalendarDay> calendarDays, int organizationId) : this(payrates, organizationId)
        {
            _branches = branches;
            _calendars = calendarDays.GroupBy(t => t.CalendarID).ToDictionary(t => t.Key, t => new PayratesCalendar(t));
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

            var branch = _branches.FirstOrDefault(t => t.RowID == branchId.Value);

            if (branch.CalendarID == null || !_calendars.ContainsKey(branch.CalendarID))
                return _organizationCalendar;

            return _calendars[branch.CalendarID];
        }
    }
}