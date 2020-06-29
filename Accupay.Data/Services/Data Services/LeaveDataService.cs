using AccuPay.Data.Entities;
using AccuPay.Data.Enums;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.ValueObjects;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class LeaveDataService : BaseDataService<Leave>
    {
        private List<string> VALIDATABLE_TYPES = new List<string>()
        {
            ProductConstant.SICK_LEAVE,
            ProductConstant.VACATION_LEAVE
        };

        private readonly PayrollContext _context;
        private readonly PolicyHelper _policy;

        private readonly EmployeeRepository _employeeRepository;
        private readonly EmployeeDutyScheduleRepository _employeeDutyScheduleRepository;
        private readonly LeaveRepository _leaveRepository;
        private readonly LeaveLedgerRepository _leaveLedgerRepository;
        private readonly PayPeriodRepository _payPeriodRepository;
        private readonly ShiftScheduleRepository _shiftScheduleRepository;

        public LeaveDataService(
            PayrollContext context,
            PolicyHelper policy,
            EmployeeRepository employeeRepository,
            EmployeeDutyScheduleRepository employeeDutyScheduleRepository,
            LeaveRepository leaveRepository,
            LeaveLedgerRepository leaveLedgerRepository,
            PayPeriodRepository payPeriodRepository,
            ShiftScheduleRepository shiftScheduleRepository) : base(leaveRepository)
        {
            _context = context;
            _policy = policy;
            _employeeRepository = employeeRepository;
            _employeeDutyScheduleRepository = employeeDutyScheduleRepository;
            _leaveRepository = leaveRepository;
            _leaveLedgerRepository = leaveLedgerRepository;
            _payPeriodRepository = payPeriodRepository;
            _shiftScheduleRepository = shiftScheduleRepository;
        }

        public async Task DeleteAsync(int leaveId)
        {
            var leave = await _leaveRepository.GetByIdAsync(leaveId);

            if (leave == null)
                throw new BusinessLogicException("Leave does not exists.");

            await _leaveRepository.DeleteAsync(leave);
        }

        protected override async Task SanitizeEntity(Leave leave)
        {
            if (leave.OrganizationID == null)
                throw new BusinessLogicException("Organization is required.");

            if (leave.EmployeeID == null)
                throw new BusinessLogicException("Employee is required.");

            if (leave.StartDate == null)
                throw new BusinessLogicException("Start Date is required.");

            if (leave.LeaveType == null)
                throw new BusinessLogicException("Leave Type is required.");

            if ((leave.StartTime.HasValue && leave.EndTime == null) || (leave.EndTime.HasValue && leave.StartTime == null))
                throw new BusinessLogicException("Both Start Time and End Time should have value or both should be empty.");

            if (new string[] { Leave.StatusPending, Leave.StatusApproved }
                            .Contains(leave.Status) == false)
            {
                throw new BusinessLogicException("Status is not valid.");
            }

            var doesExistQuery = _context.Leaves
                .Where(l => l.EmployeeID == leave.EmployeeID)
                .Where(l => l.StartDate.Date == leave.StartDate.Date);

            if (IsNewEntity(leave.RowID) == false)
            {
                doesExistQuery = doesExistQuery.Where(l => leave.RowID != l.RowID);
            }

            if (await doesExistQuery.AnyAsync())
                throw new BusinessLogicException($"Employee already has a leave for {leave.StartDate.ToShortDateString()}");

            if (leave.StartTime.HasValue)
            {
                leave.StartTime = leave.StartTime.Value.StripSeconds();
            }

            if (leave.EndTime.HasValue)
            {
                leave.EndTime = leave.EndTime.Value.StripSeconds();
            }

            if (leave.IsWholeDay == false && leave.StartTime == leave.EndTime)
                throw new BusinessLogicException("End Time cannot be equal to Start Time");

            leave.UpdateEndDate();
        }

        #region SaveManyAsync

        public override async Task SaveAsync(Leave leave)
        {
            await SaveManyAsync(new List<Leave> { leave });
        }

        public override async Task SaveManyAsync(List<Leave> leaves)
        {
            if (leaves.Any() == false) return;

            int organizationId = leaves
                .Where(x => x.OrganizationID.HasValue)
                .Select(x => x.OrganizationID.Value)
                .FirstOrDefault();

            foreach (var leave in leaves)
            {
                await SanitizeEntity(leave);
            }

            await SaveLeaves(leaves, organizationId);
        }

        private async Task SaveLeaves(List<Leave> leaves, int organizationId)
        {
            var leaveRepository = new LeaveRepository(_context);

            var employeeShifts = new List<ShiftSchedule>();
            var shiftSchedules = new List<EmployeeDutySchedule>();
            var employees = new List<Employee>();

            var orderedLeaves = leaves.OrderBy(l => l.StartDate).ToList();

            var firstLeave = leaves.FirstOrDefault().StartDate;
            var lastLeave = leaves.LastOrDefault().StartDate;

            if (_policy.ValidateLeaveBalance)
            {
                var employeeIds = leaves
                    .Where(x => x.EmployeeID.HasValue)
                    .Select(l => l.EmployeeID.Value)
                    .Distinct()
                    .ToArray();

                employeeShifts = await GetEmployeeShifts(employeeIds, organizationId, firstLeave, lastLeave);
                shiftSchedules = await GetShiftSchedules(employeeIds, organizationId, firstLeave, lastLeave);
                employees = await GetEmployees(employeeIds);
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                int?[] oldRowIds = leaves.Select(x => x.RowID).ToArray();
                try
                {
                    foreach (var leave in leaves)
                    {
                        await leaveRepository.SaveAsync(leave);

                        if (Validatable(leave))
                        {
                            var employee = employees.FirstOrDefault(e => e.RowID == leave.EmployeeID);

                            var unusedApprovedLeaves = await GetUnusedApprovedLeavesByType(
                                leaveRepository,
                                employee.RowID,
                                leave,
                                organizationId);

                            var earliestUnusedApprovedLeave = unusedApprovedLeaves
                                .OrderBy(l => l.StartDate)
                                .FirstOrDefault();

                            // if the earliest unused approved leave is earlier than the first leave, get its shifts
                            if (earliestUnusedApprovedLeave != null &&
                                earliestUnusedApprovedLeave.StartDate.ToMinimumHourValue() < firstLeave.ToMinimumHourValue())
                            {
                                var firstShiftDate = earliestUnusedApprovedLeave.StartDate.ToMinimumHourValue();
                                var lastShiftDate = firstLeave.ToMinimumHourValue().AddSeconds(-1);

                                if (leave.EmployeeID.HasValue)
                                {
                                    var earlierEmployeeShifts = await GetEmployeeShifts(
                                        new int[] { leave.EmployeeID.Value },
                                        organizationId,
                                        firstShiftDate,
                                        lastShiftDate);

                                    var earlierShiftSchedules = await GetShiftSchedules(
                                        new int[] { leave.EmployeeID.Value },
                                        organizationId,
                                        firstShiftDate,
                                        lastShiftDate);

                                    employeeShifts.InsertRange(0, earlierEmployeeShifts);
                                    shiftSchedules.InsertRange(0, earlierShiftSchedules);
                                }

                                employeeShifts = employeeShifts.OrderBy(s => s.EffectiveFrom).ToList();
                                shiftSchedules = shiftSchedules.OrderBy(s => s.DateSched).ToList();
                            }

                            await ValidateLeaveBalance(employeeShifts, shiftSchedules, unusedApprovedLeaves, employee, leave);
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    // If the exception happens after the repository SaveAsync, leaves that are to be created will have RowIDs already
                    // by the point it reaches this catch block because of SQL Transaction. Those leaves' RowIDs should be reverted back
                    // to null
                    int index = 0;
                    foreach (var leave in leaves)
                    {
                        leave.RowID = oldRowIds[index];

                        if (leave.RowID == null)
                        {
                            leave.Created = DateTime.MinValue;
                        }

                        index++;
                    }
                    throw;
                }
            }
        }

        private async Task<List<ShiftSchedule>> GetEmployeeShifts(
                int[] employeeIds,
                int organizationId,
                DateTime firstLeave,
                DateTime lastLeave)
        {
            return (await _shiftScheduleRepository.GetByEmployeeAndDatePeriodAsync(
                organizationId,
                employeeIds,
                new TimePeriod(firstLeave, lastLeave)))
                .ToList();
        }

        private async Task<List<EmployeeDutySchedule>> GetShiftSchedules(
            int[] employeeIds,
            int organizationId,
            DateTime firstLeave,
            DateTime lastLeave)
        {
            return (await _employeeDutyScheduleRepository.GetByEmployeeAndDatePeriodAsync(
                organizationId,
                employeeIds,
                new TimePeriod(firstLeave, lastLeave)))
                .ToList();
        }

        private async Task ValidateLeaveBalance(
            List<ShiftSchedule> employeeShifts,
            List<EmployeeDutySchedule> shiftSchedules,
            List<Leave> unusedApprovedLeaves,
            Employee employee,
            Leave leave)
        {
            if (employee.RowID == null)
                throw new BusinessLogicException("Employee does not exists.");

            if (Validatable(leave))
            {
                var totalLeaveHours = ComputeTotalLeaveHours(
                    unusedApprovedLeaves,
                    employeeShifts,
                    shiftSchedules,
                    employee);

                if (leave.LeaveType.ToTrimmedLowerCase() == ProductConstant.SICK_LEAVE.ToTrimmedLowerCase())
                {
                    var sickLeaveBalance = await _employeeRepository.GetSickLeaveBalance(employee.RowID.Value);

                    if (totalLeaveHours > sickLeaveBalance)
                        throw new BusinessLogicException("Employee will exceed the allowable sick leave hours.");
                }
                else if (leave.LeaveType.ToTrimmedLowerCase() == ProductConstant.VACATION_LEAVE.ToTrimmedLowerCase())
                {
                    var vacationLeaveBalance = await _employeeRepository.GetVacationLeaveBalance(employee.RowID.Value);

                    if (totalLeaveHours > vacationLeaveBalance)
                        throw new BusinessLogicException("Employee will exceed the allowable vacation leave hours.");
                }
            }
        }

        private bool Validatable(Leave leave)
        {
            var types = VALIDATABLE_TYPES.Select(x => x.ToTrimmedLowerCase());
            return leave.Status.ToTrimmedLowerCase() == Leave.StatusApproved.ToTrimmedLowerCase() &&
                    _policy.ValidateLeaveBalance && types.Contains(leave.LeaveType.ToTrimmedLowerCase());
        }

        private decimal ComputeTotalLeaveHours(
            List<Leave> leaves,
            List<ShiftSchedule> employeeShifts,
            List<EmployeeDutySchedule> shiftSchedules,
            Employee employee)
        {
            if (leaves.Any() == false)
                return 0;

            var computeBreakTimeLate = _policy.ComputeBreakTimeLate;

            decimal totalHours = 0;

            foreach (var leave in leaves)
            {
                var currentDate = leave.StartDate;

                var employeeShift = employeeShifts
                    .Where(s => s.EffectiveFrom <= currentDate)
                    .Where(s => currentDate <= s.EffectiveTo)
                    .FirstOrDefault();

                var dutyShiftSched = shiftSchedules.FirstOrDefault(es => es.DateSched == currentDate);

                var currentShift = DayCalculator.GetCurrentShift(
                    currentDate,
                    employeeShift,
                    dutyShiftSched,
                    _policy.UseShiftSchedule,
                    _policy.RespectDefaultRestDay,
                    employee.DayOfRest);

                totalHours += DayCalculator.ComputeLeaveHoursWithoutTimelog(
                    currentShift,
                    leave,
                    computeBreakTimeLate);
            }

            return totalHours;
        }

        private async Task<List<Leave>> GetUnusedApprovedLeavesByType(
            LeaveRepository leaveRepository,
            int? employeeId,
            Leave leave,
            int organizationId)
        {
            var currentPayPeriod = await _payPeriodRepository.GetByDateAsync(leave.StartDate, organizationId);

            if (currentPayPeriod == null)
                return new List<Leave>();

            var firstDayOfTheYear = await _payPeriodRepository.GetFirstDayOfTheYear(currentPayPeriod, organizationId);

            var lastDayOfTheYear = await _payPeriodRepository.GetLastDayOfTheYear(currentPayPeriod, organizationId);

            if (firstDayOfTheYear == null || lastDayOfTheYear == null)
                return new List<Leave>();

            return (await leaveRepository.GetUnusedApprovedLeavesByTypeAsync(
                    employeeId: employeeId.Value,
                    leave: leave,
                    firstDayOfTheYear: firstDayOfTheYear.Value,
                    lastDayOfTheYear: lastDayOfTheYear.Value))
                .ToList();
        }

        private async Task<List<Employee>> GetEmployees(int[] employeeIds)
        {
            return (await _employeeRepository.GetByMultipleIdAsync(employeeIds)).ToList();
        }

        #endregion SaveManyAsync

        // TODO: refactor this method to use repositories

        #region ForceUpdateLeaveAllowanceAsync

        public async Task<decimal> ForceUpdateLeaveAllowanceAsync(
            int employeeId,
            int organizationId,
            int userId,
            LeaveType selectedLeaveType,
            decimal newAllowance)
        {
            decimal newBalance = newAllowance;

            var currentPayPeriod = await _payPeriodRepository.GetCurrentPayPeriodAsync(organizationId);

            var firstPayPeriodOfTheYear = await _payPeriodRepository
                .GetFirstPayPeriodOfTheYear(
                    currentPayPeriod,
                    organizationId);

            var firstDayOfTheWorkingYear = firstPayPeriodOfTheYear?.PayFromDate;

            if (currentPayPeriod == null || firstPayPeriodOfTheYear?.RowID == null || firstDayOfTheWorkingYear == null)
                throw new Exception("Cannot retrieve current pay period or the first days of the working year.");

            var employee = await _employeeRepository.GetByIdAsync(employeeId);

            // #1. Update employee's leave allowance
            // #2. Update employee's leave transactions

            var leaveLedgerQuery = _context.LeaveLedgers
                .Include(l => l.Product)
                .Where(l => l.EmployeeID == employeeId);

            // #1
            UpdateEmployeeLeaveAllowanceAndUpdateLeaveLedgerQuery(
                selectedLeaveType,
                newAllowance,
                employee,
                leaveLedgerQuery);

            var leaveLedger = await leaveLedgerQuery.FirstOrDefaultAsync();
            var leaveLedgerId = leaveLedger?.RowID;

            if (leaveLedgerId == null)
                throw new Exception("Cannot retrieve the leave ledger ID.");

            Console.WriteLine($"Leave ledger ID: {leaveLedgerId}");

            var leaveTransactions = await _context.LeaveTransactions
                .Where(l => l.EmployeeID == employeeId)
                .Where(l => l.TransactionDate >= firstDayOfTheWorkingYear.Value)
                .Where(l => l.LeaveLedgerID == leaveLedgerId)
                .OrderBy(l => l.TransactionDate)
                .ToListAsync();

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
            await ProcessIfEmployeeHasNotTakenAleaveThisYear(
                firstPayPeriodOfTheYear,
                leaveLedgerId: leaveLedgerId.Value,
                userId: userId);

            return newBalance;
        }

        private void UpdateEmployeeLeaveAllowanceAndUpdateLeaveLedgerQuery(
            LeaveType selectedLeaveType,
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

        private async Task ProcessIfEmployeeHasNotTakenAleaveThisYear(
            PayPeriod firstPayPeriodOfTheYear,
            int leaveLedgerId,
            int userId)
        {
            var updatedLeaveLedger = await _context.LeaveLedgers.FirstOrDefaultAsync(l => l.RowID == leaveLedgerId);

            if (updatedLeaveLedger == null)
            {
                throw new ArgumentException("Cannot find leave ledger.");
            }
            else if (updatedLeaveLedger.LastTransactionID == null)
            {
                // get the beginning balance transaction geting it from the first payperiod of the year
                // that we added earlier [using GUID as rowids would have made this easier]
                var updatedBeginningTransaction = await _context.LeaveTransactions
                    .Where(t => t.PayPeriodID == firstPayPeriodOfTheYear.RowID)
                    .Where(t => t.LeaveLedgerID == updatedLeaveLedger.RowID)
                    .Where(t => t.IsCredit).OrderByDescending(t => t.Amount)
                    .FirstOrDefaultAsync();

                if (updatedBeginningTransaction == null)
                    throw new ArgumentException("Was not able to create a beginning transaction");

                updatedLeaveLedger.LastTransactionID = updatedBeginningTransaction.RowID;
                updatedLeaveLedger.LastUpdBy = userId;
                await _context.SaveChangesAsync();
            }
        }

        #endregion ForceUpdateLeaveAllowanceAsync

        public async Task<PaginatedListResult<LeaveTransaction>> ListTransactions(PageOptions options, int organizationId, int id, string type)
        {
            return await _leaveLedgerRepository.ListTransactions(options, organizationId, id, type);
        }

        public async Task<PaginatedListResult<LeaveLedger>> GetLeaveBalances(PageOptions options, int organizationId, string searchTerm)
        {
            var leaveBalances = await _leaveLedgerRepository.GetLeaveBalance(organizationId, searchTerm);

            var distinctId = leaveBalances.Select(x => x.EmployeeID).Distinct().AsQueryable();

            var ids = distinctId.Page(options).ToList();

            var query = leaveBalances
                .Where(x => ids.Contains(x.EmployeeID))
                .ToList();

            var count = distinctId.Count();

            return new PaginatedListResult<LeaveLedger>(query, count);
        }
    }
}