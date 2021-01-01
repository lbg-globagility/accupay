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
    public class LeaveDataService : BaseDailyPayrollDataService<Leave>
    {
        private const string UserActivityName = "Leave";

        private List<string> VALIDATABLE_TYPES = new List<string>()
        {
            ProductConstant.SICK_LEAVE,
            ProductConstant.VACATION_LEAVE
        };

        private readonly EmployeeRepository _employeeRepository;
        private readonly ShiftRepository _shiftRepository;
        private readonly LeaveLedgerRepository _leaveLedgerRepository;

        public LeaveDataService(
            PayrollContext context,
            IPolicyHelper policy,
            EmployeeRepository employeeRepository,
            ShiftRepository shiftRepository,
            LeaveRepository leaveRepository,
            LeaveLedgerRepository leaveLedgerRepository,
            PayPeriodRepository payPeriodRepository,
            UserActivityRepository userActivityRepository) :

            base(leaveRepository,
                payPeriodRepository,
                userActivityRepository,
                context,
                policy,
                entityName: "Leave")
        {
            _employeeRepository = employeeRepository;
            _shiftRepository = shiftRepository;
            _leaveLedgerRepository = leaveLedgerRepository;
        }

        #region SaveManyAsync

        public override async Task<Leave> SaveAsync(Leave leave, int changedByUserId)
        {
            await SaveManyAsync(new List<Leave> { leave }, changedByUserId);

            return leave;
        }

        public override async Task SaveManyAsync(List<Leave> leaves, int changedByUserId)
        {
            if (leaves.Any() == false) return;

            var oldLeaves = await GetOldEntitiesAsync(leaves);

            int organizationId = leaves
                .Where(x => x.OrganizationID.HasValue)
                .Select(x => x.OrganizationID.Value)
                .FirstOrDefault();

            foreach (var leave in leaves)
            {
                var oldLeave = oldLeaves.Where(x => x.RowID == leave.RowID).FirstOrDefault();
                await SanitizeEntity(leave, oldLeave, changedByUserId);
            }

            await ValidateDates(leaves, oldLeaves.ToList(), organizationId);

            var newLeaves = leaves.Where(x => x.IsNewEntity).ToList();
            var updatedLeaves = leaves.Where(x => !x.IsNewEntity).ToList();

            await SaveLeavesAsync(leaves, organizationId);

            await PostSaveManyAction(newLeaves, new List<Leave>(), SaveType.Insert, changedByUserId);
            await PostSaveManyAction(updatedLeaves, oldLeaves.ToList(), SaveType.Update, changedByUserId);
        }

        private async Task SaveLeavesAsync(List<Leave> leaves, int organizationId)
        {
            var leaveRepository = new LeaveRepository(_context);

            var shifts = new List<Shift>();
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

                shifts = await GetShifts(employeeIds, organizationId, firstLeave, lastLeave);
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
                                    var earlierShifts = await GetShifts(
                                        new int[] { leave.EmployeeID.Value },
                                        organizationId,
                                        firstShiftDate,
                                        lastShiftDate);

                                    shifts.InsertRange(0, earlierShifts);
                                }

                                shifts = shifts.OrderBy(s => s.DateSched).ToList();
                            }

                            await ValidateLeaveBalance(shifts, unusedApprovedLeaves, employee, leave);
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

                        index++;
                    }
                    throw;
                }
            }
        }

        private async Task<List<Shift>> GetShifts(
            int[] employeeIds,
            int organizationId,
            DateTime firstLeave,
            DateTime lastLeave)
        {
            return (await _shiftRepository.GetByEmployeeAndDatePeriodAsync(
                organizationId,
                employeeIds,
                new TimePeriod(firstLeave, lastLeave)))
                .ToList();
        }

        private async Task ValidateLeaveBalance(
            List<Shift> shifts,
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
                    shifts,
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
            List<Shift> shifts,
            Employee employee)
        {
            if (leaves.Any() == false)
                return 0;

            var computeBreakTimeLate = _policy.ComputeBreakTimeLate;

            decimal totalHours = 0;

            foreach (var leave in leaves)
            {
                var currentDate = leave.StartDate;

                var shift = shifts.FirstOrDefault(es => es.DateSched == currentDate);

                var currentShift = DayCalculator.GetCurrentShift(
                    currentDate,
                    shift,
                    _policy.RespectDefaultRestDay,
                    employee.DayOfRest,
                    _policy.ShiftBasedAutomaticOvertimePolicy);

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
            var currentPayPeriod = await _payPeriodRepository.GetAsync(organizationId, leave.StartDate);

            if (currentPayPeriod == null)
                return new List<Leave>();

            var firstDayOfTheYear = await _payPeriodRepository.GetFirstDayOfTheYear(
                currentPayPeriodYear: currentPayPeriod.Year,
                organizationId: organizationId);

            var lastDayOfTheYear = await _payPeriodRepository.GetLastDayOfTheYear(
                currentPayPeriodYear: currentPayPeriod.Year,
                organizationId: organizationId);

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
            const string cannotRetrieveDataErrorMessage = "Cannot retrieve current pay period or the first days of the working year.";

            var currentPayPeriod = await _payPeriodRepository.GetOrCreateCurrentPayPeriodAsync(
                organizationId: organizationId,
                currentUserId: userId);

            if (currentPayPeriod == null)
                throw new BusinessLogicException(cannotRetrieveDataErrorMessage);

            var firstPayPeriodOfTheYear = await _payPeriodRepository
                .GetFirstPayPeriodOfTheYear(
                currentPayPeriodYear: currentPayPeriod.Year,
                organizationId: organizationId);

            var firstDayOfTheWorkingYear = firstPayPeriodOfTheYear?.PayFromDate;

            if (firstPayPeriodOfTheYear?.RowID == null || firstDayOfTheWorkingYear == null)
                throw new BusinessLogicException(cannotRetrieveDataErrorMessage);

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

        public async Task<PaginatedList<LeaveLedger>> GetLeaveBalancesAsync(PageOptions options, int organizationId, string searchTerm)
        {
            var leaveBalances = await _leaveLedgerRepository.GetLeaveBalancesAsync(organizationId, searchTerm);

            var distinctId = leaveBalances.Select(x => x.EmployeeID).Distinct().AsQueryable();

            var ids = distinctId.Page(options).ToList();

            var query = leaveBalances
                .Where(x => ids.Contains(x.EmployeeID))
                .ToList();

            var count = distinctId.Count();

            return new PaginatedList<LeaveLedger>(query, count);
        }

        #region Overrides

        protected override string GetUserActivityName(Leave leave)
        {
            return UserActivityName;
        }

        protected override string CreateUserActivitySuffixIdentifier(Leave leave)
        {
            return $" with date '{leave.StartDate.ToShortDateString()}'";
        }

        protected override async Task SanitizeEntity(Leave leave, Leave oldLeave, int changedByUserId)
        {
            await base.SanitizeEntity(
                entity: leave,
                oldEntity: oldLeave,
                currentlyLoggedInUserId: changedByUserId);

            if (leave.StartDate < PayrollTools.SqlServerMinimumDate)
                throw new BusinessLogicException("Date cannot be earlier than January 1, 1753.");

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

            if (leave.IsNewEntity == false)
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

        protected override async Task RecordUpdate(Leave newValue, Leave oldValue)
        {
            var changes = new List<UserActivityItem>();
            var entityName = UserActivityName.ToLower();

            var suffixIdentifier = $"of {entityName}{CreateUserActivitySuffixIdentifier(oldValue)}.";

            if (newValue.StartDate != oldValue.StartDate)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated start date from '{oldValue.StartDate.ToShortDateString()}' to '{newValue.StartDate.ToShortDateString()}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            if (newValue.StartTime.ToString() != oldValue.StartTime.ToString())
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated start time from '{oldValue.StartTime.ToStringFormat("hh:mm tt")}' to '{newValue.StartTime.ToStringFormat("hh:mm tt")}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            if (newValue.EndTime.ToString() != oldValue.EndTime.ToString())
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated end time from '{oldValue.EndTime.ToStringFormat("hh:mm tt")}' to '{newValue.EndTime.ToStringFormat("hh:mm tt")}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            if (newValue.Reason != oldValue.Reason)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated reason from '{oldValue.Reason}' to '{newValue.Reason}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            if (newValue.Comments != oldValue.Comments)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated comments from '{oldValue.Comments}' to '{newValue.Comments}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            if (newValue.LeaveType != oldValue.LeaveType)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated type from '{oldValue.LeaveType}' to '{newValue.LeaveType}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            if (newValue.Status != oldValue.Status)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated status from '{oldValue.Status}' to '{newValue.Status}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });

            if (changes.Any())
            {
                await _userActivityRepository.CreateRecordAsync(
                    newValue.LastUpdBy.Value,
                    UserActivityName,
                    newValue.OrganizationID.Value,
                    UserActivityRepository.RecordTypeEdit,
                    changes);
            }
        }

        #endregion Overrides
    }
}
