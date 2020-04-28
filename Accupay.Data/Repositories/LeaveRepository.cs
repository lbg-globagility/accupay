using AccuPay.Data.Entities;
using AccuPay.Data.Enums;
using AccuPay.Data.Helpers;
using AccuPay.Data.Services;
using AccuPay.Data.ValueObjects;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class LeaveRepository
    {
        private EmployeeRepository _employeeRepository;

        public LeaveRepository()
        {
            _employeeRepository = new EmployeeRepository();
        }

        private List<string> VALIDATABLE_TYPES = new List<string>()
        {
            ProductConstant.SICK_LEAVE,
            ProductConstant.VACATION_LEAVE
        };

        #region CRUD

        public async Task DeleteAsync(int id)
        {
            using (var context = new PayrollContext())
            {
                var leave = await GetByIdAsync(id);

                context.Remove(leave);

                await context.SaveChangesAsync();
            }
        }

        // TODO: maybe move this to a service will all the validations
        public async Task SaveManyAsync(List<Leave> leaves, int organizationId)
        {
            if (leaves.Any() == false) return;

            PolicyHelper policy = new PolicyHelper();

            List<ShiftSchedule> employeeShifts = new List<ShiftSchedule>();
            List<EmployeeDutySchedule> shiftSchedules = new List<EmployeeDutySchedule>();
            List<Employee> employees = new List<Employee>();

            var orderedLeaves = leaves.OrderBy(l => l.StartDate).ToList();

            var firstLeave = leaves.FirstOrDefault().StartDate;
            var lastLeave = leaves.LastOrDefault().StartDate;

            using (PayrollContext context = new PayrollContext())
            {
                if (policy.ValidateLeaveBalance)
                {
                    var employeeIds = leaves.Select(l => l.EmployeeID).Distinct();

                    employeeShifts = await GetEmployeeShifts(employeeIds, organizationId, firstLeave, lastLeave, context);
                    shiftSchedules = await GetShiftSchedules(employeeIds, organizationId, firstLeave, lastLeave, context);
                    employees = await GetEmployees(employeeIds);
                }

                await context.Leaves.LoadAsync();

                foreach (var leave in leaves)
                {
                    await SaveWithContextAsync(leave, context);

                    if (policy.ValidateLeaveBalance)
                    {
                        var employee = employees.FirstOrDefault(e => e.RowID == leave.EmployeeID);

                        var unusedApprovedLeaves = await GetUnusedApprovedLeavesByType(context,
                                                                                        employee.RowID,
                                                                                        leave,
                                                                                        organizationId);

                        var earliestUnusedApprovedLeave = unusedApprovedLeaves.
                                                            OrderBy(l => l.StartDate).
                                                            FirstOrDefault();

                        // if the earliest unused approved leave is earlier than the first leave, get its shifts
                        if (earliestUnusedApprovedLeave != null &&
                                earliestUnusedApprovedLeave.StartDate.ToMinimumHourValue() <
                                                            firstLeave.ToMinimumHourValue())
                        {
                            var firstShiftDate = earliestUnusedApprovedLeave.StartDate.ToMinimumHourValue();
                            var lastShiftDate = firstLeave.ToMinimumHourValue().AddSeconds(-1);

                            var earlierEmployeeShifts = await GetEmployeeShifts(
                                                                new int?[] { leave.EmployeeID },
                                                                organizationId,
                                                                firstShiftDate,
                                                                lastShiftDate,
                                                                context);

                            var earlierShiftSchedules = await GetShiftSchedules(
                                                                new int?[] { leave.EmployeeID },
                                                                organizationId,
                                                                firstShiftDate,
                                                                lastShiftDate,
                                                                context);

                            employeeShifts.InsertRange(0, earlierEmployeeShifts);
                            shiftSchedules.InsertRange(0, earlierShiftSchedules);

                            employeeShifts = employeeShifts.OrderBy(s => s.EffectiveFrom).ToList();
                            shiftSchedules = shiftSchedules.OrderBy(s => s.DateSched).ToList();
                        }

                        await ValidateLeaveBalance(policy, employeeShifts, shiftSchedules, unusedApprovedLeaves, employee, leave);
                    }
                }

                await context.SaveChangesAsync();
            }
        }

        public async Task SaveAsync(Leave leave)
        {
            await SaveWithContextAsync(leave);
        }

        private async Task SaveWithContextAsync(Leave leave, PayrollContext passedContext = null)
        {
            if (leave.StartTime.HasValue)
                leave.StartTime = leave.StartTime.Value.StripSeconds();
            if (leave.EndTime.HasValue)
                leave.EndTime = leave.EndTime.Value.StripSeconds();

            if (passedContext == null)
            {
                using (var newContext = new PayrollContext())
                {
                    await SaveAsyncFunction(leave, newContext);
                    await newContext.SaveChangesAsync();
                }
            }
            else
            {
                await SaveAsyncFunction(leave, passedContext);
            }
        }

        private async Task SaveAsyncFunction(Leave leave, PayrollContext context)
        {
            if (await context.Leaves.Where(l => leave.RowID == null ? true : leave.RowID != l.RowID).
                                Where(l => l.EmployeeID == leave.EmployeeID).
                                Where(l => (leave.StartDate.Date >= l.StartDate.Date && leave.StartDate.Date <= l.EndDate.Value.Date) ||
                                        (leave.EndDate.Value.Date >= l.StartDate.Date && leave.EndDate.Value.Date <= l.EndDate.Value.Date)).
                                AnyAsync())
                throw new ArgumentException($"Employee already has a leave for {leave.StartDate.ToShortDateString()}");

            if (leave.RowID == null)
            {
                context.Leaves.Add(leave);
            }
            else
            {
                // since we used LoadAsync() above, we can't just simply attach the leave
                // TODO: refactor the validate leave above to not load the whole database
                // better if we use transactions then check afterwards if there are invariants
                // just like in metrotiles
                await UpdateAsync(leave, context);
            }
        }

        private async Task UpdateAsync(Leave leave, PayrollContext context)
        {
            var currentLeave = await context.Leaves.
                                FirstOrDefaultAsync(l => l.RowID == leave.RowID);

            if (currentLeave == null) return;

            currentLeave.StartTime = leave.StartTime;
            currentLeave.EndTime = leave.EndTime;
            currentLeave.LeaveType = leave.LeaveType;
            currentLeave.StartDate = leave.StartDate;
            currentLeave.EndDate = leave.EndDate;
            currentLeave.Reason = leave.Reason;
            currentLeave.Comments = leave.Comments;
            currentLeave.Status = leave.Status;
            currentLeave.LastUpdBy = leave.LastUpdBy;
        }

        public async Task<decimal> ForceUpdateLeaveAllowance(int employeeId,
                                                            int organizationId,
                                                            int userId,
                                                            LeaveType selectedLeaveType,
                                                            decimal newAllowance)
        {
            decimal newBalance = newAllowance;

            var currentPayPeriod = await PayrollTools.GetCurrentlyWorkedOnPayPeriodByCurrentYear(organizationId);

            var firstPayPeriodOfTheYear = await PayrollTools.GetFirstPayPeriodOfTheYear(context: null,
                                                                                        currentPayPeriod: (PayPeriod)currentPayPeriod,
                                                                                        organizationId);

            var firstDayOfTheWorkingYear = firstPayPeriodOfTheYear?.PayFromDate;

            if (currentPayPeriod == null || firstPayPeriodOfTheYear?.RowID == null || firstDayOfTheWorkingYear == null)
                throw new Exception("Cannot retrieve current pay period or the first days of the working year.");

            var employee = await _employeeRepository.GetByIdAsync(employeeId);

            using (PayrollContext context = new PayrollContext())
            {
                // #1. Update employee's leave allowance
                // #2. Update employee's leave transactions

                var leaveLedgerQuery = context.LeaveLedgers.
                                                Include(l => l.Product).
                                                Where(l => l.EmployeeID == employeeId);

                // #1
                UpdateEmployeeLeaveAllowanceAndUpdateLeaveLedgerQuery(selectedLeaveType,
                                                                        newAllowance,
                                                                        employee,
                                                                        leaveLedgerQuery);

                var leaveLedger = await leaveLedgerQuery.FirstOrDefaultAsync();
                var leaveLedgerId = leaveLedger?.RowID;

                if (leaveLedgerId == null)
                    throw new Exception("Cannot retrieve the leave ledger ID.");

                Console.WriteLine($"Leave ledger ID: {leaveLedgerId}");

                var leaveTransactions = await context.LeaveTransactions.
                                                    Where(l => l.EmployeeID == employeeId).
                                                    Where(l => l.TransactionDate >= firstDayOfTheWorkingYear.Value).
                                                    Where(l => l.LeaveLedgerID == leaveLedgerId).
                                                    OrderBy(l => l.TransactionDate).
                                                    ToListAsync();

                // 2.1. Add the first credit (the beginning balance).
                // 2.2. Remove existing credits from database. It should only have one credit, on the first cutoff, then all debits.
                // 2.3. Save all debits but their balance should be recalculated to adjust to the new allowance.
                // 2.4. Update the leaveledger's last transaction ID.
                // 2.5. Check if there is a last transaction ID saved for the leaveledger.
                // If no last transaction ID, this means that the employee did not have a leave for the current pay period.
                // Use the newly added first credit (the beginning balance) as the last transaction ID for the leaveledger.

                int? lastTransactionId = null;

                // #2.1
                LeaveTransaction beginningTransaction = new LeaveTransaction();
                beginningTransaction.OrganizationID = organizationId;
                beginningTransaction.CreatedBy = userId;
                beginningTransaction.EmployeeID = employeeId;
                beginningTransaction.ReferenceID = null;
                beginningTransaction.LeaveLedgerID = leaveLedgerId;
                beginningTransaction.PayPeriodID = firstPayPeriodOfTheYear.RowID;
                beginningTransaction.TransactionDate = firstDayOfTheWorkingYear.Value;
                beginningTransaction.Type = LeaveTransactionType.Credit;
                beginningTransaction.Amount = newAllowance;
                beginningTransaction.Balance = newAllowance;

                context.LeaveTransactions.Add(beginningTransaction);

                // -
                foreach (var leaveTransaction in leaveTransactions)
                {
                    if (leaveTransaction.IsCredit)

                        // #2.2
                        context.Remove(leaveTransaction);
                    else
                    {
                        // #2.3
                        newBalance = newBalance - leaveTransaction.Amount;
                        leaveTransaction.Balance = newBalance;
                        leaveTransaction.LastUpdBy = userId;

                        lastTransactionId = leaveTransaction.RowID;
                    }
                }

                // #2.4
                // lastTransactionId will be null if the employee did not use at least one leave for the
                // whole year. This will be managed in section 2.5
                leaveLedger.LastTransactionID = lastTransactionId;
                leaveLedger.LastUpdBy = userId;

                await context.SaveChangesAsync();

                // #2.5
                await ProcessIfEmployeeHasNotTakenAleaveThisYear(firstPayPeriodOfTheYear,
                                                                context,
                                                                leaveLedgerId.Value,
                                                                userId);
            }

            return newBalance;
        }

        private static async Task ProcessIfEmployeeHasNotTakenAleaveThisYear(PayPeriod firstPayPeriodOfTheYear,
                                                                            PayrollContext context,
                                                                            int leaveLedgerId,
                                                                            int userId)
        {
            var updatedLeaveLedger = await context.LeaveLedgers.
                                            FirstOrDefaultAsync(l => l.RowID == leaveLedgerId);

            if (updatedLeaveLedger == null)
                throw new ArgumentException("Cannot find leave ledger.");
            else if (updatedLeaveLedger.LastTransactionID == null)
            {
                // get the beginning balance transaction geting it from the first payperiod of the year
                // that we added earlier [using GUID as rowids would have made this easier]
                var updatedBeginningTransaction = await context.LeaveTransactions.
                                Where(t => t.PayPeriodID == firstPayPeriodOfTheYear.RowID).
                                Where(t => t.LeaveLedgerID == updatedLeaveLedger.RowID).
                                Where(t => t.IsCredit).OrderByDescending(t => t.Amount).
                                FirstOrDefaultAsync();

                if (updatedBeginningTransaction == null)
                    throw new ArgumentException("Was not able to create a beginning transaction");

                updatedLeaveLedger.LastTransactionID = updatedBeginningTransaction.RowID;
                updatedLeaveLedger.LastUpdBy = userId;
                await context.SaveChangesAsync();
            }
        }

        #endregion CRUD

        #region Queries

        #region Single entity

        public async Task<Leave> GetByIdAsync(int id)
        {
            using (var context = new PayrollContext())
            {
                return await context.Leaves.FirstOrDefaultAsync(l => l.RowID == id);
            }
        }

        #endregion Single entity

        #region List of entities

        public async Task<IEnumerable<Leave>> GetByEmployeeAsync(int employeeId)
        {
            using (var context = new PayrollContext())
            {
                return await context.Leaves.
                                    Where(l => l.EmployeeID == employeeId).
                                    ToListAsync();
            }
        }

        public IEnumerable<Leave> GetAllApprovedByDatePeriod(int organizationId, TimePeriod timePeriod)
        {
            using (var context = new PayrollContext(PayrollContext.DbCommandConsoleLoggerFactory))
            {
                return CreateBaseQueryByTimePeriod(organizationId, timePeriod, context).
                                Where(l => l.Status.Trim().ToLower() == Leave.StatusApproved.ToTrimmedLowerCase()).
                                ToList();
            }
        }

        public async Task<IEnumerable<Leave>> GetAllByTimePeriodAsync(int organizationId, TimePeriod timePeriod)
        {
            using (var context = new PayrollContext())
            {
                return await CreateBaseQueryByTimePeriod(organizationId, timePeriod, context).
                                ToListAsync();
            }
        }

        public async Task<IEnumerable<Leave>> GetFilteredAllAsync(Expression<Func<Leave, bool>> filter)
        {
            using (var context = new PayrollContext())
            {
                return await context.Leaves.Where(filter).ToListAsync();
            }
        }

        #endregion List of entities

        #region Others

        public List<string> GetStatusList()
        {
            return new List<string>()
            {
                Leave.StatusPending,
                Leave.StatusApproved
            };
        }

        #endregion Others

        #endregion Queries

        #region Private helper methods

        private async Task ValidateLeaveBalance(PolicyHelper policy,
                                                List<ShiftSchedule> employeeShifts,
                                                List<EmployeeDutySchedule> shiftSchedules,
                                                List<Leave> unusedApprovedLeaves,
                                                Employee employee,
                                                Leave leave)
        {
            if (employee.RowID == null)
                throw new ArgumentException("Employee does not exists.");

            if (leave.Status.ToTrimmedLowerCase() == Leave.StatusApproved.ToTrimmedLowerCase() &&
                policy.ValidateLeaveBalance && VALIDATABLE_TYPES.Contains(leave.LeaveType))
            {
                var totalLeaveHours = ComputeTotalLeaveHours(unusedApprovedLeaves,
                                                            policy,
                                                            employeeShifts,
                                                            shiftSchedules,
                                                            employee);

                if (leave.LeaveType == ProductConstant.SICK_LEAVE)
                {
                    var sickLeaveBalance = await EmployeeData.GetSickLeaveBalance(employee.RowID.Value);

                    if (totalLeaveHours > sickLeaveBalance)
                        throw new ArgumentException("Employee will exceed the allowable sick leave hours.");
                }
                else if (leave.LeaveType == ProductConstant.VACATION_LEAVE)
                {
                    var vacationLeaveBalance = await EmployeeData.GetVacationLeaveBalance(employee.RowID.Value);

                    if (totalLeaveHours > vacationLeaveBalance)
                        throw new ArgumentException("Employee will exceed the allowable vacation leave hours.");
                }
            }
        }

        private async Task<List<Employee>> GetEmployees(IEnumerable<int?> employeeIds)
        {
            var ids = employeeIds.Select(id => id).ToList();
            return (await _employeeRepository.GetByMultipleIdAsync(ids)).ToList();
        }

        private static async Task<List<EmployeeDutySchedule>> GetShiftSchedules(IEnumerable<int?> employeeIds,
                                                                                int organizationId,
                                                                                DateTime firstLeave,
                                                                                DateTime lastLeave,
                                                                                PayrollContext context)
        {
            return await context.EmployeeDutySchedules.
                                    Where(es => es.OrganizationID == organizationId).
                                    Where(es => firstLeave <= es.DateSched).
                                    Where(es => es.DateSched <= lastLeave).
                                    Where(es => employeeIds.Contains(es.EmployeeID)).
                                    ToListAsync();
        }

        private static async Task<List<ShiftSchedule>> GetEmployeeShifts(IEnumerable<int?> employeeIds,
                                                                        int organizationId,
                                                                        DateTime firstLeave,
                                                                        DateTime lastLeave,
                                                                        PayrollContext context)
        {
            return await context.ShiftSchedules.
                                    Include(s => s.Shift).Where(s => s.OrganizationID == organizationId).
                                    Where(s => s.EffectiveFrom <= lastLeave).
                                    Where(s => firstLeave <= s.EffectiveTo).
                                    Where(s => employeeIds.Contains(s.EmployeeID)).
                                    ToListAsync();
        }

        private decimal ComputeTotalLeaveHours(List<Leave> leaves,
                                                PolicyHelper policy,
                                                List<ShiftSchedule> employeeShifts,
                                                List<EmployeeDutySchedule> shiftSchedules,
                                                Employee employee)
        {
            if (leaves.Any() == false)
                return 0;

            var computeBreakTimeLate = policy.ComputeBreakTimeLate;

            decimal totalHours = 0;

            foreach (var leave in leaves)
            {
                var currentDate = leave.StartDate;

                var employeeShift = employeeShifts.Where(s => s.EffectiveFrom <= currentDate).
                                                    Where(s => currentDate <= s.EffectiveTo).
                                                    FirstOrDefault();

                var dutyShiftSched = shiftSchedules.FirstOrDefault(es => es.DateSched == currentDate);

                var currentShift = DayCalculator.GetCurrentShift(currentDate,
                                                                employeeShift,
                                                                dutyShiftSched,
                                                                policy.UseShiftSchedule,
                                                                policy.RespectDefaultRestDay,
                                                                employee.DayOfRest);

                totalHours += DayCalculator.ComputeLeaveHoursWithoutTimelog(currentShift,
                                                                            leave,
                                                                            computeBreakTimeLate);
            }

            return totalHours;
        }

        private void UpdateEmployeeLeaveAllowanceAndUpdateLeaveLedgerQuery(LeaveType selectedLeaveType,
                                                                            decimal newAllowance,
                                                                            Employee employee,
                                                                            IQueryable<LeaveLedger> leaveLedgerQuery)
        {
            switch (selectedLeaveType)
            {
                case LeaveType.Sick:
                    leaveLedgerQuery = leaveLedgerQuery.Where(l => l.Product.IsSickLeave);
                    employee.SickLeaveAllowance = newAllowance;
                    break;

                case LeaveType.Vacation:
                    leaveLedgerQuery = leaveLedgerQuery.Where(l => l.Product.IsVacationLeave);
                    employee.VacationLeaveAllowance = newAllowance;
                    break;

                case LeaveType.Others:
                    leaveLedgerQuery = leaveLedgerQuery.Where(l => l.Product.IsOthersLeave);
                    employee.OtherLeaveAllowance = newAllowance;
                    break;

                case LeaveType.Maternity:
                    leaveLedgerQuery = leaveLedgerQuery.Where(l => l.Product.IsMaternityLeave);
                    employee.MaternityLeaveAllowance = newAllowance;
                    break;

                case LeaveType.Parental:
                    leaveLedgerQuery = leaveLedgerQuery.Where(l => l.Product.IsParentalLeave);
                    // THIS DOES NOT HAVE AN ALLOWANCE COLUMN
                    throw new Exception("No column for Parental Leave Allowance on employee table.");
            }
        }

        private static async Task<List<Leave>> GetUnusedApprovedLeavesByType(PayrollContext context, int? employeeId, Leave leave, int organizationId)
        {
            var currentPayPeriod = context.PayPeriods.
                                            Where(p => p.OrganizationID == organizationId).
                                            Where(p => p.IsSemiMonthly).
                                            Where(p => p.IsBetween(leave.StartDate)).
                                            FirstOrDefault();

            if (currentPayPeriod == null)
                return new List<Leave>();

            DateTime? firstDayOfTheYear = await PayrollTools.GetFirstDayOfTheYear(context, currentPayPeriod, organizationId);

            var lastDayOfTheYear = context.PayPeriods.
                                            Where(p => p.OrganizationID == organizationId).
                                            Where(p => p.IsSemiMonthly).
                                            Where(p => p.Year == currentPayPeriod.Year).
                                            Where(p => p.IsLastPayPeriodOfTheYear).
                                            Select(p => p.PayToDate).
                                            FirstOrDefault();

            if (firstDayOfTheYear == null || lastDayOfTheYear == null)
                return new List<Leave>();

            return context.Leaves.Local.
                            Where(l => l.EmployeeID == employeeId).
                            Where(l => l.Status.Trim().ToUpper() == Leave.StatusApproved.ToUpper()).
                            Where(l => l.LeaveType == leave.LeaveType).
                            Where(l => l.StartDate >= firstDayOfTheYear.Value).
                            Where(l => l.StartDate <= lastDayOfTheYear).
                            Where(l => context.LeaveTransactions.
                                                Where(t => t.ReferenceID == l.RowID).
                                                Count() == 0).
                           ToList();
        }

        private static IQueryable<Leave> CreateBaseQueryByTimePeriod(int organizationId,
                                                                    TimePeriod timePeriod,
                                                                    PayrollContext context)
        {
            return context.Leaves.
                        Where(l => l.OrganizationID == organizationId).
                        Where(l => timePeriod.Start <= l.StartDate).
                        Where(l => l.EndDate <= timePeriod.End);
        }

        #endregion Private helper methods
    }
}