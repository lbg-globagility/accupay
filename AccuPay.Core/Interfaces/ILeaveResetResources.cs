using AccuPay.Core.Entities;
using AccuPay.Core.Services;
using AccuPay.Core.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ILeaveResetResources
    {
        Task Load(int organizationId, int userId, TimePeriod timePeriod);

        IReadOnlyCollection<ActualTimeEntry> ActualTimeEntries { get; }
        IReadOnlyCollection<Allowance> Allowances { get; }
        IReadOnlyCollection<Bonus> Bonuses { get; }
        Product BpiInsuranceProduct { get; }
        CalendarCollection CalendarCollection { get; }
        string CurrentSystemOwner { get; }
        IReadOnlyCollection<Employee> Employees { get; }
        IReadOnlyCollection<Leave> Leaves { get; }
        ListOfValueCollection ListOfValueCollection { get; }
        IReadOnlyCollection<Loan> Loans { get; }
        PayPeriod PayPeriod { get; }
        IReadOnlyCollection<Paystub> Paystubs { get; }
        IPolicyHelper Policy { get; }
        IReadOnlyCollection<Paystub> PreviousPaystubs { get; }
        IReadOnlyCollection<Salary> Salaries { get; }
        Product SickLeaveProduct { get; }
        IReadOnlyCollection<SocialSecurityBracket> SocialSecurityBrackets { get; }
        IReadOnlyCollection<TimeEntry> TimeEntries { get; }
        Product VacationLeaveProduct { get; }
        IReadOnlyCollection<WithholdingTaxBracket> WithholdingTaxBrackets { get; }
        IReadOnlyCollection<Shift> Shifts { get; }
        IReadOnlyCollection<LeaveLedger> LeaveLedgers { get; }
        IReadOnlyCollection<Product> LeaveTypes { get; }
        IReadOnlyCollection<CashoutUnusedLeave> CashoutUnusedLeaves { get; }
    }
}
