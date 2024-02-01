using AccuPay.Core.Entities;
using AccuPay.Core.Entities.LeaveReset;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Services.LeaveBalanceReset;
using AccuPay.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class LeaveBalanceResetCalculator : ILeaveBalanceResetCalculator
    {
        private readonly ILeaveLedgerRepository _leaveLedgerRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUserActivityRepository _userActivityRepository;

        public LeaveBalanceResetCalculator(ILeaveLedgerRepository leaveLedgerRepository,
            IEmployeeRepository employeeRepository,
            IUserActivityRepository userActivityRepository)
        {
            _leaveLedgerRepository = leaveLedgerRepository;
            _employeeRepository = employeeRepository;
            _userActivityRepository = userActivityRepository;
        }

        public async Task<LeaveResetResult> Start(int organizationId,
            int userId,
            int employeeId,
            LeaveReset leaveReset,
            ILeaveResetResources resources)
        {
            var employee = resources.Employees.
                Where(x => x.RowID == employeeId).
                FirstOrDefault();

            var timeEntries = resources.TimeEntries.
                Where(t => t.EmployeeID == employeeId).
                Where(t => t.TotalLeaveHours > 0).
                OrderBy(t => t.Date).
                ToList();

            var leaveLedgers = resources.LeaveLedgers.
                Where(t => t.EmployeeID == employeeId).
                ToList();

            var leaveTypes = resources.LeaveTypes.ToList();

            var tenure = leaveReset.GetLeaveTenureTier(employee: employee);

            var newUserActivityItems = new List<UserActivityItem>();

            try
            {
                //LeaveTypeEnum[] leaveTypeNames = {
                //    leaveReset.IsVacationLeaveSupported ? LeaveTypeEnum.Vacation : null,
                //    leaveReset.IsSickLeaveSupported ? LeaveTypeEnum.Sick : null,
                //    leaveReset.IsOthersLeaveSupported ? LeaveTypeEnum.Others : null,
                //    leaveReset.IsParentalLeaveSupported ? LeaveTypeEnum.Parental : null};
                LeaveTypeEnum[] leaveTypeNames = {
                    LeaveTypeEnum.Vacation,
                    LeaveTypeEnum.Sick,
                    LeaveTypeEnum.Others,
                    LeaveTypeEnum.Parental};

                var supportedLeaveTypeNames = leaveTypeNames.
                    Where(n => n != null);

                var leaveResetYearRangePeriod = leaveReset.YearRangePeriod;

                var updatedLeaveLedgers = new List<LeaveLedger>();
                var newLeaveTransactions = new List<LeaveTransaction>();
                var deletableLeaveTransactions = new List<LeaveTransaction>();

                foreach (var leaveTypeName in supportedLeaveTypeNames)
                {
                    var leaveLedger = leaveLedgers.
                        FirstOrDefault(l => l.LeaveTypeName == leaveTypeName.Type);

                    var updatedLeaveTransactions = PerformReset(leaveTypeName: leaveTypeName,
                        employee: employee,
                        tenure: tenure,
                        timeEntries: timeEntries,
                        leaveReset: leaveReset,
                        leaveTypes: leaveTypes,
                        leaveLedger: leaveLedger,
                        userId: userId,
                        employeeId: employeeId,
                        organizationId: organizationId,
                        userActivityItems: ref newUserActivityItems);
                    if (updatedLeaveTransactions == null) continue;

                    leaveLedger.LastTransaction = updatedLeaveTransactions.LastOrDefault();
                    
                    var discardLeaveTransactions = leaveLedger.LeaveTransactions.
                        Where(lt => lt.TransactionDate >= leaveResetYearRangePeriod.Start).
                        Where(lt => lt.TransactionDate <= leaveResetYearRangePeriod.End);
                    deletableLeaveTransactions.AddRange(discardLeaveTransactions);

                    updatedLeaveLedgers.Add(leaveLedger);

                    newLeaveTransactions.AddRange(updatedLeaveTransactions);
                }

                await _leaveLedgerRepository.DeleteManyLeaveTransactionsAsync(leaveTransactions: deletableLeaveTransactions);

                await _leaveLedgerRepository.CreateManyLeaveTransactionsAsync(leaveTransactions: newLeaveTransactions);

                await _leaveLedgerRepository.UpdateManyAsync(leaveLedgers: updatedLeaveLedgers);

                await _employeeRepository.UpdateAsync(employee);

                await _userActivityRepository.CreateRecordAsync(currentlyLoggedInUserId:userId,
                    entityName: "EMPLOYEE",
                    organizationId: organizationId,
                    recordType: "EDIT",
                    activityItems: newUserActivityItems);

                return LeaveResetResult.Success(employee, "Successfully reset leave");
            }
            catch (LeaveResetException ex)
            {
                return LeaveResetResult.Error(employee, ex.Message);
            }
            catch (Exception ex)
            {
                return LeaveResetResult.Error(employee, $"Failure reset leave for employee {employee.EmployeeNo + Environment.NewLine} {ex.Message}");
            }
        }

        private List<LeaveTransaction> PerformReset(LeaveTypeEnum leaveTypeName,
            Employee employee,
            LeaveTenure tenure,
            List<TimeEntry> timeEntries,
            LeaveReset leaveReset,
            List<Product> leaveTypes,
            LeaveLedger leaveLedger,
            int userId,
            int employeeId,
            int organizationId,
            ref List<UserActivityItem> userActivityItems)
        {
            var isLeaveSupported = leaveReset.IsLeaveTypeSupprted(leaveTypeName: leaveTypeName);
            //if (!isLeaveSupported) return null;


            decimal tenureLeaveHours = 0;
            switch (leaveTypeName.Type)
            {
                case ProductConstant.VACATION_LEAVE:
                    tenureLeaveHours = tenure.GetVacationLeaveHours(employee: employee,
                        yearRangePeriod: leaveReset.YearRangePeriod);
                    break;

                case ProductConstant.SICK_LEAVE:
                    tenureLeaveHours = tenure.GetSickLeaveHours(employee: employee,
                        yearRangePeriod: leaveReset.YearRangePeriod);
                    break;

                case ProductConstant.OTHERS_LEAVE:
                    tenureLeaveHours = tenure.GetOthersLeaveHours(employee: employee,
                        yearRangePeriod: leaveReset.YearRangePeriod);
                    break;

                case ProductConstant.PARENT_LEAVE:
                    tenureLeaveHours = tenure.GetParentalLeaveHours(employee: employee,
                        yearRangePeriod: leaveReset.YearRangePeriod);
                    break;

                default:
                    tenureLeaveHours = 0;
                    break;
            }

            var leaveTypeId = leaveTypes.
                FirstOrDefault(p => p.PartNo == leaveTypeName.Type).
                RowID.Value;

            // put beginning leave allowance/balance
            //await _leaveLedgerRepository.CreateBeginningBalanceAsync(employeeId: employeeId,
            //    leaveTypeId: leaveTypeId,
            //    userId: userId,
            //    organizationId: organizationId,
            //    balance: tenureLeaveHours,
            //    description: "Beginning balance",
            //    transactionDate: leaveReset.StartPeriodDate);

            // put updated leave balance
            decimal filedLeaveHours = 0;

            decimal decrementTenureLeaveHours = tenureLeaveHours;

            var leaveTransactions = new List<LeaveTransaction>();

            var beginningLeaveTransaction = LeaveTransaction.NewLeaveTransaction(userId: userId,
                organizationId: organizationId,
                employeeId: employeeId,
                leaveLedgerId: leaveLedger?.RowID,
                payPeriodId: null,
                paystubId: null,
                referenceId: null,
                transactionDate: leaveReset.StartPeriodDate,
                description: "Beginning balance",
                type: LeaveTransactionType.Credit,
                amount: tenureLeaveHours,
                balance: tenureLeaveHours);
            leaveTransactions.Add(beginningLeaveTransaction);

            switch (leaveTypeName.Type)
            {
                case ProductConstant.VACATION_LEAVE:
                    filedLeaveHours = timeEntries.Sum(t => t.VacationLeaveHours);
                    foreach (var timeEntry in timeEntries)
                    {
                        var leaveHours = timeEntry.VacationLeaveHours;
                        decrementTenureLeaveHours -= leaveHours;

                        leaveTransactions.Add(CreateLeaveTransaction(leaveLedger,
                            userId,
                            employeeId,
                            organizationId,
                            decrementTenureLeaveHours,
                            timeEntry,
                            leaveHours));
                    }
                    break;

                case ProductConstant.SICK_LEAVE:
                    filedLeaveHours = timeEntries.Sum(t => t.SickLeaveHours);
                    foreach (var timeEntry in timeEntries)
                    {
                        var leaveHours = timeEntry.SickLeaveHours;
                        decrementTenureLeaveHours -= leaveHours;

                        leaveTransactions.Add(CreateLeaveTransaction(leaveLedger,
                            userId,
                            employeeId,
                            organizationId,
                            decrementTenureLeaveHours,
                            timeEntry,
                            leaveHours));
                    }
                    break;

                case ProductConstant.OTHERS_LEAVE:
                    filedLeaveHours = timeEntries.Sum(t => t.OtherLeaveHours);
                    foreach (var timeEntry in timeEntries)
                    {
                        var leaveHours = timeEntry.OtherLeaveHours;
                        decrementTenureLeaveHours -= leaveHours;

                        leaveTransactions.Add(CreateLeaveTransaction(leaveLedger,
                            userId,
                            employeeId,
                            organizationId,
                            decrementTenureLeaveHours,
                            timeEntry,
                            leaveHours));
                    }
                    break;

                case ProductConstant.PARENT_LEAVE:
                    filedLeaveHours = timeEntries.Sum(t => t.MaternityLeaveHours);
                    foreach (var timeEntry in timeEntries)
                    {
                        var leaveHours = timeEntry.MaternityLeaveHours;
                        decrementTenureLeaveHours -= leaveHours;

                        leaveTransactions.Add(CreateLeaveTransaction(leaveLedger,
                            userId,
                            employeeId,
                            organizationId,
                            decrementTenureLeaveHours,
                            timeEntry,
                            leaveHours));
                    }
                    break;

                default:
                    filedLeaveHours = 0;
                    break;
            }

            // update employee profile leave balance
            var updatedLeaveBalance = tenureLeaveHours - filedLeaveHours;
            switch (leaveTypeName.Type)
            {
                case ProductConstant.VACATION_LEAVE:
                    var origVacationLeaveAllowance = employee.VacationLeaveAllowance;
                    var origLeaveBalance = AccuMath.CommercialRound(employee.LeaveBalance);
                    employee.VacationLeaveAllowance = tenureLeaveHours;// tenure.VacationLeaveHours;
                    employee.LeaveBalance = updatedLeaveBalance;
                    if (isLeaveSupported)
                    {
                        userActivityItems.Add(NewUserActivityItem(entityId: employeeId,
                        description: $"(ResetLeave): vacation leave allowance changed from {origVacationLeaveAllowance} to {AccuMath.CommercialRound(tenureLeaveHours)}",
                        employeeId: employeeId,
                        userId: userId));
                        userActivityItems.Add(NewUserActivityItem(entityId: employeeId,
                            description: $"(ResetLeave): vacation leave balance from {origLeaveBalance} to {AccuMath.CommercialRound(updatedLeaveBalance)}",
                            employeeId: employeeId,
                            userId: userId));
                    }
                    break;

                case ProductConstant.SICK_LEAVE:
                    var origSickLeaveAllowance = employee.SickLeaveAllowance;
                    var origSickLeaveBalance = AccuMath.CommercialRound(employee.SickLeaveBalance);
                    employee.SickLeaveAllowance = tenureLeaveHours;//tenure.SickLeaveHours;
                    employee.SickLeaveBalance = updatedLeaveBalance;
                    if (isLeaveSupported)
                    {
                        userActivityItems.Add(NewUserActivityItem(entityId: employeeId,
                        description: $"(ResetLeave): sick leave allowance changed from {origSickLeaveAllowance} to {AccuMath.CommercialRound(tenureLeaveHours)}",
                        employeeId: employeeId,
                        userId: userId));
                        userActivityItems.Add(NewUserActivityItem(entityId: employeeId,
                            description: $"(ResetLeave): sick leave balance from {origSickLeaveBalance} to {AccuMath.CommercialRound(updatedLeaveBalance)}",
                            employeeId: employeeId,
                            userId: userId));
                    }
                    break;

                case ProductConstant.OTHERS_LEAVE:
                    var origOtherLeaveAllowance = employee.OtherLeaveAllowance;
                    var origOtherLeaveBalance = AccuMath.CommercialRound(employee.OtherLeaveBalance);
                    employee.OtherLeaveAllowance = tenureLeaveHours;//tenure.OthersLeaveHours;
                    employee.SetOtherLeaveBalance(otherLeaveBalance: updatedLeaveBalance);
                    if (isLeaveSupported)
                    {
                        userActivityItems.Add(NewUserActivityItem(entityId: employeeId,
                        description: $"(ResetLeave): others leave allowance changed from {origOtherLeaveAllowance} to {AccuMath.CommercialRound(tenureLeaveHours)}",
                        employeeId: employeeId,
                        userId: userId));
                        userActivityItems.Add(NewUserActivityItem(entityId: employeeId,
                            description: $"(ResetLeave): others leave balance from {origOtherLeaveBalance} to {AccuMath.CommercialRound(updatedLeaveBalance)}",
                            employeeId: employeeId,
                            userId: userId));
                    }
                    break;

                case ProductConstant.PARENT_LEAVE:
                    var origParentalLeaveAllowance = employee.MaternityLeaveAllowance;
                    var origParentalLeaveBalance = AccuMath.CommercialRound(employee.MaternityLeaveBalance);
                    employee.MaternityLeaveAllowance = tenureLeaveHours;//tenure.ParentalLeaveHours;
                    employee.SetParentalLeaveBalance(parentalLeaveBalance: updatedLeaveBalance);
                    if(isLeaveSupported)
                    {
                        userActivityItems.Add(NewUserActivityItem(entityId: employeeId,
                            description: $"(ResetLeave): parental leave allowance changed from {origParentalLeaveAllowance} to {AccuMath.CommercialRound(tenureLeaveHours)}",
                            employeeId: employeeId,
                            userId: userId));
                        userActivityItems.Add(NewUserActivityItem(entityId: employeeId,
                            description: $"(ResetLeave): parental leave balance from {origParentalLeaveBalance} to {AccuMath.CommercialRound(updatedLeaveBalance)}",
                            employeeId: employeeId,
                            userId: userId));
                    }
                    break;

                default:
                    break;
            }
            employee.LastUpdBy = userId;

            if (leaveTransactions.Any())
            {
                //await _leaveLedgerRepository.AppendLeaveTransactionAsync(employeeId: employeeId,
                //    leaveTypeId: leaveTypeId,
                //    userId: userId,
                //    organizationId: organizationId,
                //    transactionDate: leaveReset.StartPeriodDate,
                //    amount: filedLeaveHours,
                //    balance: tenureLeaveHours - filedLeaveHours);

                //await _leaveLedgerRepository.AppendLeaveTransactionsAsync(employeeId: employeeId,
                //    leaveTypeId: leaveTypeId,
                //    userId: userId,
                //    organizationId: organizationId,
                //    leaveTransactions: leaveTransactions);

                //await _leaveLedgerRepository.AppendLeaveTransactionsAsync(leaveLedger: leaveLedger,
                //    leaveTransactions: leaveTransactions);
            }

            return leaveTransactions;
        }

        private LeaveTransaction CreateLeaveTransaction(LeaveLedger leaveLedger,
            int userId,
            int employeeId,
            int organizationId,
            decimal decrementTenureLeaveHours,
            TimeEntry timeEntry,
            decimal leaveHours)
        {
            return LeaveTransaction.NewLeaveTransaction(userId: userId,
                organizationId: organizationId,
                employeeId: employeeId,
                leaveLedgerId: leaveLedger?.RowID,
                payPeriodId: null,
                paystubId: null,
                referenceId: null,
                transactionDate: timeEntry.Date,
                description: string.Empty,
                type: LeaveTransactionType.Debit,
                amount: leaveHours,
                balance: decrementTenureLeaveHours);
        }

        private UserActivityItem NewUserActivityItem(int entityId,
            string description,
            int? employeeId,
            int? userId)
        {
            int len = description.Length > 2000 ? 2000 : description.Length;
            return new UserActivityItem() {
                Created = DateTime.Now,
                EntityId = entityId,
                Description = description.Substring(0, len),
                ChangedEmployeeId = employeeId,
                ChangedUserId = userId
            };
        }
    }
}
