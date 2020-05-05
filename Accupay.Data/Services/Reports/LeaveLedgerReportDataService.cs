using AccuPay.Data.Entities;
using AccuPay.Data.Enums;
using AccuPay.Data.ReportModels;
using AccuPay.Data.Repositories;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class LeaveLedgerReportDataService
    {
        private readonly int _organizationId;
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        private readonly EmployeeRepository _employeeRepository;

        public LeaveLedgerReportDataService(int organizationId, DateTime dateFrom, DateTime dateTo)
        {
            this._organizationId = organizationId;
            this._startDate = dateFrom.ToMinimumHourValue();
            this._endDate = dateTo.ToMinimumHourValue();

            _employeeRepository = new EmployeeRepository();
        }

        public async Task<List<LeaveLedgerReportModel>> GetData()
        {
            var leaveLedgerReportModels = new List<LeaveLedgerReportModel>();

            var dayBeforeReport = _startDate.AddDays(-1).ToMaximumHourValue();

            var employees = await _employeeRepository.GetAllActiveAsync(_organizationId);
            var employeeIds = employees.Select(e => e.RowID).ToArray();

            using (PayrollContext context = new PayrollContext())
            {
                var oldLeaveTransactions = await context.LeaveTransactions.
                                                    Include(l => l.LeaveLedger).
                                                    Include(l => l.LeaveLedger.Product).
                                                    Where(l => l.TransactionDate <= dayBeforeReport).
                                                    Where(l => l.LeaveLedger.Product.IsVacationOrSickLeave).
                                                    Where(l => employeeIds.Contains(l.EmployeeID)).
                                                    OrderBy(l => l.TransactionDate).
                                                    ToListAsync();

                var currentLeaveTransactions = await context.LeaveTransactions.
                                                    Include(l => l.LeaveLedger).
                                                    Include(l => l.LeaveLedger.Product).
                                                    Where(l => l.TransactionDate >= _startDate).
                                                    Where(l => l.TransactionDate <= _endDate).
                                                    Where(l => l.LeaveLedger.Product.IsVacationOrSickLeave).
                                                    Where(l => employeeIds.Contains(l.EmployeeID)).
                                                    OrderBy(l => l.TransactionDate).
                                                    ToListAsync();

                var timeEntries = await context.TimeEntries.
                                            Where(t => t.Date >= _startDate).
                                            Where(t => t.Date <= _endDate).
                                            Where(t => employeeIds.Contains(t.EmployeeID)).
                                            OrderBy(l => l.Date).ToListAsync();

                foreach (var employee in employees)
                {
                    var vacationLeave = GetVacationLeave(oldLeaveTransactions, currentLeaveTransactions, timeEntries, employee);

                    var sickLeave = GetSickLeave(oldLeaveTransactions, currentLeaveTransactions, timeEntries, employee);

                    leaveLedgerReportModels.Add(vacationLeave);

                    leaveLedgerReportModels.Add(sickLeave);
                }
            }

            return leaveLedgerReportModels;
            //return new List<ILeaveLedgerReportModel>(leaveLedgerReportModels);
        }

        private static LeaveLedgerReportModel GetVacationLeave(List<LeaveTransaction> oldLeaveTransactions,
                                                            List<LeaveTransaction> currentLeaveTransactions,
                                                            List<TimeEntry> timeEntries,
                                                            Employee employee)
        {
            var currentEmployeeLeaveTransactions = currentLeaveTransactions.
                                                    Where(l => l.EmployeeID == employee.RowID).
                                                    Where(l => l.LeaveLedger?.Product?.IsVacationLeave ?? false).
                                                    ToList();

            var oldEmployeeLeaveTransactions = oldLeaveTransactions.
                                                    Where(l => l.EmployeeID == employee.RowID).
                                                    Where(l => l.LeaveLedger?.Product?.IsVacationLeave ?? false).
                                                    ToList();

            var employeeTimeEntries = timeEntries.Where(t => t.EmployeeID == employee.RowID).
                                                    Where(t => t.VacationLeaveHours > 0).
                                                    ToList();

            return GetLeave(oldLeaveTransactions: oldEmployeeLeaveTransactions,
                            currentLeaveTransactions: currentEmployeeLeaveTransactions,
                            timeEntries: employeeTimeEntries,
                            employee: employee,
                            leaveType: LeaveType.Vacation);
        }

        private static LeaveLedgerReportModel GetSickLeave(List<LeaveTransaction> oldLeaveTransactions,
                                                        List<LeaveTransaction> currentLeaveTransactions,
                                                        List<TimeEntry> timeEntries,
                                                        Employee employee)
        {
            var currentEmployeeLeaveTransactions = currentLeaveTransactions.
                                                    Where(l => l.EmployeeID == employee.RowID).
                                                    Where(l => l.LeaveLedger?.Product?.IsSickLeave ?? false).
                                                    ToList();

            var oldEmployeeLeaveTransactions = oldLeaveTransactions.
                                                    Where(l => l.EmployeeID == employee.RowID).
                                                    Where(l => l.LeaveLedger?.Product?.IsSickLeave ?? false).
                                                    ToList();

            var employeeTimeEntries = timeEntries.Where(t => t.EmployeeID == employee.RowID).
                                                    Where(t => t.SickLeaveHours > 0).
                                                    ToList();

            return GetLeave(oldLeaveTransactions: oldEmployeeLeaveTransactions,
                            currentLeaveTransactions: currentEmployeeLeaveTransactions,
                            timeEntries: employeeTimeEntries,
                            employee: employee,
                            leaveType: LeaveType.Vacation);
        }

        private static LeaveLedgerReportModel GetLeave(List<LeaveTransaction> oldLeaveTransactions,
                                                    List<LeaveTransaction> currentLeaveTransactions,
                                                    List<TimeEntry> timeEntries,
                                                    Employee employee,
                                                    LeaveType leaveType)
        {
            LeaveTransaction leaveBeginningTransaction = new LeaveTransaction();

            // check first if leave was reset during the report period
            leaveBeginningTransaction = currentLeaveTransactions.Where(l => l.IsCredit).LastOrDefault();

            if (leaveBeginningTransaction != null)
            {
                // if it was reset, only get the leaves that are after the reset date
                var resetDate = leaveBeginningTransaction.TransactionDate.ToMinimumHourValue();
                timeEntries = timeEntries.Where(t => t.Date >= resetDate).ToList();
            }
            else
                // if it was not reset during the report period, get the last leavetransaction before the report period
                leaveBeginningTransaction = oldLeaveTransactions.LastOrDefault();

            var leaveBeginningBalance = leaveBeginningTransaction?.Balance ?? 0;

            // get total availed leave
            decimal totalAvailedLeave = 0;

            if (leaveType == LeaveType.Vacation)
                totalAvailedLeave = timeEntries.Sum(t => t.VacationLeaveHours);
            else if (leaveType == LeaveType.Sick)
                totalAvailedLeave = timeEntries.Sum(t => t.SickLeaveHours);

            // return LeaveLedgerReportModel object
            return new LeaveLedgerReportModel(employeeNumber: employee.EmployeeNo,
                                            fullName: employee.FullNameWithMiddleInitialLastNameFirst,
                                            leaveType: leaveType,
                                            beginningBalance: leaveBeginningBalance,
                                            availedLeave: totalAvailedLeave);
        }
    }
}