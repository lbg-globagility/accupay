using AccuPay.Core.Entities;
using AccuPay.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ITimeEntryResources
    {
        Task Load(int organizationId, DateTime cutoffStart, DateTime cutoffEnd);

        IReadOnlyCollection<ActualTimeEntry> ActualTimeEntries { get; }
        IReadOnlyCollection<Agency> Agencies { get; }
        IReadOnlyCollection<AgencyFee> AgencyFees { get; }
        IReadOnlyCollection<BreakTimeBracket> BreakTimeBrackets { get; }
        CalendarCollection CalendarCollection { get; }
        IReadOnlyCollection<Employee> Employees { get; }
        IReadOnlyCollection<EmploymentPolicy> EmploymentPolicies { get; }
        IReadOnlyCollection<Leave> Leaves { get; }
        IReadOnlyCollection<OfficialBusiness> OfficialBusinesses { get; }
        Organization Organization { get; }
        IReadOnlyCollection<Overtime> Overtimes { get; }
        IPolicyHelper Policy { get; }
        IReadOnlyCollection<RoutePayRate> RouteRates { get; }
        IReadOnlyCollection<Salary> Salaries { get; }
        IReadOnlyCollection<Shift> Shifts { get; }
        IReadOnlyCollection<TimeAttendanceLog> TimeAttendanceLogs { get; }
        IReadOnlyCollection<TimeEntry> TimeEntries { get; }
        IReadOnlyCollection<TimeLog> TimeLogs { get; }
        IReadOnlyCollection<TripTicket> TripTickets { get; }
        IReadOnlyCollection<AllowanceSalaryTimeEntry> AllowanceSalaryTimeEntries { get; }
    }
}
