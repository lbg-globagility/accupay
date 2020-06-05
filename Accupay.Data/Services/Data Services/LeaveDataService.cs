using AccuPay.Data.Entities;
using AccuPay.Data.Enums;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class LeaveDataService
    {
        private List<string> VALIDATABLE_TYPES = new List<string>()
        {
            ProductConstant.SICK_LEAVE,
            ProductConstant.VACATION_LEAVE
        };

        private readonly PayrollContext _context;
        private readonly PolicyHelper _policy;

        private readonly EmployeeRepository _employeeRepository;
        private readonly PayPeriodRepository _payPeriodRepository;

        public LeaveDataService(PayrollContext context,
                                PolicyHelper policy,
                                EmployeeRepository employeeRepository,
                                PayPeriodRepository payPeriodRepository)
        {
            _context = context;
            _policy = policy;
            _employeeRepository = employeeRepository;
            _payPeriodRepository = payPeriodRepository;
        }

        #region SaveManyAsync

        public async Task SaveManyAsync(List<Leave> leaves, int organizationId)
        {
            if (leaves.Any() == false) return;

            // we want to share this service's context to LeaveRepository
            // constructor injecting leaveRepository would give them separate context
            LeaveRepository leaveRepository = new LeaveRepository(_context);

            List<ShiftSchedule> employeeShifts = new List<ShiftSchedule>();
            List<EmployeeDutySchedule> shiftSchedules = new List<EmployeeDutySchedule>();
            List<Employee> employees = new List<Employee>();

            var orderedLeaves = leaves.OrderBy(l => l.StartDate).ToList();

            var firstLeave = leaves.FirstOrDefault().StartDate;
            var lastLeave = leaves.LastOrDefault().StartDate;

            if (_policy.ValidateLeaveBalance)
            {
                var employeeIds = leaves.Select(l => l.EmployeeID).Distinct().ToArray();
                var notNullEmployeeIds = employeeIds.Where(x => x != null).Select(x => x.Value).ToArray();

                employeeShifts = await GetEmployeeShifts(employeeIds, organizationId, firstLeave, lastLeave);
                shiftSchedules = await GetShiftSchedules(employeeIds, organizationId, firstLeave, lastLeave);
                employees = await GetEmployees(notNullEmployeeIds);
            }

            await _context.Leaves.LoadAsync();

            foreach (var leave in leaves)
            {
                await leaveRepository.SaveWithContextAsync(leave);

                if (_policy.ValidateLeaveBalance)
                {
                    var employee = employees.FirstOrDefault(e => e.RowID == leave.EmployeeID);

                    var unusedApprovedLeaves = await GetUnusedApprovedLeavesByType(employee.RowID,
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
                                                            lastShiftDate);

                        var earlierShiftSchedules = await GetShiftSchedules(
                                                            new int?[] { leave.EmployeeID },
                                                            organizationId,
                                                            firstShiftDate,
                                                            lastShiftDate);

                        employeeShifts.InsertRange(0, earlierEmployeeShifts);
                        shiftSchedules.InsertRange(0, earlierShiftSchedules);

                        employeeShifts = employeeShifts.OrderBy(s => s.EffectiveFrom).ToList();
                        shiftSchedules = shiftSchedules.OrderBy(s => s.DateSched).ToList();
                    }

                    await ValidateLeaveBalance(_policy, employeeShifts, shiftSchedules, unusedApprovedLeaves, employee, leave);
                }
            }

            await _context.SaveChangesAsync();
        }

        private async Task<List<ShiftSchedule>> GetEmployeeShifts(int?[] employeeIds,
                                                                int organizationId,
                                                                DateTime firstLeave,
                                                                DateTime lastLeave)
        {
            return await _context.ShiftSchedules.
                                    Include(s => s.Shift).Where(s => s.OrganizationID == organizationId).
                                    Where(s => s.EffectiveFrom <= lastLeave).
                                    Where(s => firstLeave <= s.EffectiveTo).
                                    Where(s => employeeIds.Contains(s.EmployeeID)).
                                    ToListAsync();
        }

        private async Task<List<EmployeeDutySchedule>> GetShiftSchedules(int?[] employeeIds,
                                                                        int organizationId,
                                                                        DateTime firstLeave,
                                                                        DateTime lastLeave)
        {
            return await _context.EmployeeDutySchedules.
                                    Where(es => es.OrganizationID == organizationId).
                                    Where(es => firstLeave <= es.DateSched).
                                    Where(es => es.DateSched <= lastLeave).
                                    Where(es => employeeIds.Contains(es.EmployeeID)).
                                    ToListAsync();
        }

        private async Task ValidateLeaveBalance(PolicyHelper policy,
                                                List<ShiftSchedule> employeeShifts,
                                                List<EmployeeDutySchedule> shiftSchedules,
                                                List<Leave> unusedApprovedLeaves,
                                                Employee employee,
                                                Leave leave)
        {
            if (employee.RowID == null)
                throw new BusinessLogicException("Employee does not exists.");

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
                    var sickLeaveBalance = await _employeeRepository.
                                                GetSickLeaveBalance(employee.RowID.Value);

                    if (totalLeaveHours > sickLeaveBalance)
                        throw new BusinessLogicException("Employee will exceed the allowable sick leave hours.");
                }
                else if (leave.LeaveType == ProductConstant.VACATION_LEAVE)
                {
                    var vacationLeaveBalance = await _employeeRepository.
                                                GetVacationLeaveBalance(employee.RowID.Value);

                    if (totalLeaveHours > vacationLeaveBalance)
                        throw new BusinessLogicException("Employee will exceed the allowable vacation leave hours.");
                }
            }
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

        private async Task<List<Leave>> GetUnusedApprovedLeavesByType(int? employeeId,
                                                                    Leave leave,
                                                                    int organizationId)
        {
            var currentPayPeriod = _context.PayPeriods.
                                            Where(p => p.OrganizationID == organizationId).
                                            Where(p => p.IsSemiMonthly).
                                            Where(p => p.IsBetween(leave.StartDate)).
                                            FirstOrDefault();

            if (currentPayPeriod == null)
                return new List<Leave>();

            DateTime? firstDayOfTheYear = await _payPeriodRepository.
                                      GetFirstDayOfTheYear(currentPayPeriod, organizationId);

            var lastDayOfTheYear = _context.PayPeriods.
                                            Where(p => p.OrganizationID == organizationId).
                                            Where(p => p.IsSemiMonthly).
                                            Where(p => p.Year == currentPayPeriod.Year).
                                            Where(p => p.IsLastPayPeriodOfTheYear).
                                            Select(p => p.PayToDate).
                                            FirstOrDefault();

            if (firstDayOfTheYear == null || lastDayOfTheYear == null)
                return new List<Leave>();

            return _context.Leaves.Local.
                            Where(l => l.EmployeeID == employeeId).
                            Where(l => l.Status.Trim().ToUpper() == Leave.StatusApproved.ToUpper()).
                            Where(l => l.LeaveType == leave.LeaveType).
                            Where(l => l.StartDate >= firstDayOfTheYear.Value).
                            Where(l => l.StartDate <= lastDayOfTheYear).
                            Where(l => _context.LeaveTransactions.
                                                Where(t => t.ReferenceID == l.RowID).
                                                Count() == 0).
                           ToList();
        }

        private async Task<List<Employee>> GetEmployees(int[] employeeIds)
        {
            return (await _employeeRepository.GetByMultipleIdAsync(employeeIds)).ToList();
        }

        #endregion SaveManyAsync

        #region ForceUpdateLeaveAllowanceAsync

        public async Task<decimal> ForceUpdateLeaveAllowanceAsync(int employeeId,
                                                                int organizationId,
                                                                int userId,
                                                                LeaveType selectedLeaveType,
                                                                decimal newAllowance)
        {
            decimal newBalance = newAllowance;

            var currentPayPeriod = await _payPeriodRepository.GetCurrentPayPeriodAsync(organizationId);

            var firstPayPeriodOfTheYear = await _payPeriodRepository.GetFirstPayPeriodOfTheYear(
                                                                        currentPayPeriod,
                                                                        organizationId);

            var firstDayOfTheWorkingYear = firstPayPeriodOfTheYear?.PayFromDate;

            if (currentPayPeriod == null || firstPayPeriodOfTheYear?.RowID == null || firstDayOfTheWorkingYear == null)
                throw new Exception("Cannot retrieve current pay period or the first days of the working year.");

            var employee = await _employeeRepository.GetByIdAsync(employeeId);

            // #1. Update employee's leave allowance
            // #2. Update employee's leave transactions

            var leaveLedgerQuery = _context.LeaveLedgers.
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

            var leaveTransactions = await _context.LeaveTransactions.
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

            _context.LeaveTransactions.Add(beginningTransaction);

            // -
            foreach (var leaveTransaction in leaveTransactions)
            {
                if (leaveTransaction.IsCredit)

                    // #2.2
                    _context.Remove(leaveTransaction);
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

            await _context.SaveChangesAsync();

            // #2.5
            await ProcessIfEmployeeHasNotTakenAleaveThisYear(firstPayPeriodOfTheYear,
                                                            leaveLedgerId: leaveLedgerId.Value,
                                                            userId: userId);

            return newBalance;
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

        private async Task ProcessIfEmployeeHasNotTakenAleaveThisYear(PayPeriod firstPayPeriodOfTheYear,
                                                                            int leaveLedgerId,
                                                                            int userId)
        {
            var updatedLeaveLedger = await _context.LeaveLedgers.
                                            FirstOrDefaultAsync(l => l.RowID == leaveLedgerId);

            if (updatedLeaveLedger == null)
                throw new ArgumentException("Cannot find leave ledger.");
            else if (updatedLeaveLedger.LastTransactionID == null)
            {
                // get the beginning balance transaction geting it from the first payperiod of the year
                // that we added earlier [using GUID as rowids would have made this easier]
                var updatedBeginningTransaction = await _context.LeaveTransactions.
                                Where(t => t.PayPeriodID == firstPayPeriodOfTheYear.RowID).
                                Where(t => t.LeaveLedgerID == updatedLeaveLedger.RowID).
                                Where(t => t.IsCredit).OrderByDescending(t => t.Amount).
                                FirstOrDefaultAsync();

                if (updatedBeginningTransaction == null)
                    throw new ArgumentException("Was not able to create a beginning transaction");

                updatedLeaveLedger.LastTransactionID = updatedBeginningTransaction.RowID;
                updatedLeaveLedger.LastUpdBy = userId;
                await _context.SaveChangesAsync();
            }
        }

        #endregion ForceUpdateLeaveAllowanceAsync
    }
}