using AccuPay.Core.Entities;
using AccuPay.Core.Services;
using AccuPay.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ICostCenterReportResources
    {
        Task Load(DateTime selectedMonth);

        List<ActualTimeEntry> ActualTimeEntries { get; }
        CalendarCollection CalendarCollection { get; }
        List<Allowance> DailyAllowances { get; }
        List<Employee> Employees { get; }
        List<LoanTransaction> HmoLoans { get; }
        List<PayPeriod> PayPeriods { get; }
        List<Paystub> Paystubs { get; }
        TimePeriod ReportPeriod { get; }
        List<TimePeriod> ReportTimePeriods { get; }
        List<Salary> Salaries { get; }
        ListOfValueCollection Settings { get; }
        List<SocialSecurityBracket> SocialSecurityBrackets { get; }
        List<TimeEntry> TimeEntries { get; }
    }
}
