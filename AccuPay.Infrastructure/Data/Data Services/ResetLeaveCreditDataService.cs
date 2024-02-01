using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Interfaces.Repositories;
using AccuPay.Core.Services.Imports.ResetLeaveCredits;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class ResetLeaveCreditDataService : BaseOrganizationDataService<ResetLeaveCredit>, IResetLeaveCreditDataService
    {
        private const string UserActivityResetLeaveCreditName = "ResetLeaveCredit";
        private readonly IResetLeaveCreditRepository _resetLeaveCreditRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILeaveLedgerRepository _leaveLedgerRepository;
        private readonly IProductRepository _productRepository;

        public ResetLeaveCreditDataService(
            IResetLeaveCreditRepository resetLeaveCreditRepository,
            IEmployeeRepository employeeRepository,
            ILeaveLedgerRepository leaveLedgerRepository,
            IProductRepository productRepository,
            IPayPeriodRepository payPeriodRepository,
            IUserActivityRepository userActivityRepository,
            PayrollContext context,
            IPolicyHelper policy) :

            base(resetLeaveCreditRepository,
                payPeriodRepository,
                userActivityRepository,
                context,
                policy,
                entityName: "ResetLeaveCredit")
        {
            _resetLeaveCreditRepository = resetLeaveCreditRepository;
            _employeeRepository = employeeRepository;
            _leaveLedgerRepository = leaveLedgerRepository;
            _productRepository = productRepository;
        }

        public async Task ApplyLeaveCredits(int organizationId, int userId, IList<ResetLeaveCreditItemModel> resetLeaveCreditItems)
        {
            var resetLeaveCreditId = resetLeaveCreditItems.FirstOrDefault().OriginalResetLeaveCreditItem.ResetLeaveCreditId.Value;
            var resetLeaveCredit = await _resetLeaveCreditRepository.GetByIdAsync(resetLeaveCreditId);
            resetLeaveCredit.LastUpdBy = userId;

            var payPeriod = await _payPeriodRepository.GetByIdAsync(resetLeaveCredit.StartPeriodId.Value);

            var employeeIds = resetLeaveCreditItems.Select(t => t.EmployeeRowID.Value).ToArray();

            var employees = await _employeeRepository.GetByMultipleIdAsync(employeeIds);

            var resetLeaveCreditItemIds = resetLeaveCreditItems.Select(t => t.Id).ToArray();
            var originalResetLeaveCreditItems = resetLeaveCredit.ResetLeaveCreditItems
                .Where(t => resetLeaveCreditItemIds.Contains(t.RowID))
                .ToList();

            string[] selectedLeaveTypes = { ProductConstant.VACATION_LEAVE, ProductConstant.SICK_LEAVE };

            var leaveTypes = (await _productRepository.GetLeaveTypesAsync(organizationId))
                .Where(p => selectedLeaveTypes.Contains(p.PartNo))
                .ToList();

            var leaveTypeIds = leaveTypes.Select(l => l.RowID).ToArray();

            var leaveLedgers = (await _leaveLedgerRepository.GetAll(organizationId))
                .Where(l => employeeIds.Contains(l.EmployeeID.Value))
                .Where(l => leaveTypeIds.Contains(l.ProductID))
                .ToList();

            foreach (var i in originalResetLeaveCreditItems)
            {
                var update = resetLeaveCreditItems.FirstOrDefault(t => t.Id == i.RowID);
                if (update == null) continue;

                var employee = employees.FirstOrDefault(e => e.RowID == update.EmployeeRowID);
                if(employee != null)
                {
                    // vacation leave
                    employee.VacationLeaveAllowance = update.VacationLeaveCredit;
                    employee.LeaveBalance = update.VacationLeaveCredit;

                    var vacationLeaveLedger = leaveLedgers
                        .Where(l => l.EmployeeID == employee.RowID)
                        .Where(l => l.Product.PartNo == ProductConstant.VACATION_LEAVE)
                        .FirstOrDefault();

                    var newVacationLeaveTransaction = LeaveTransaction.NewLeaveTransaction(userId: userId,
                        organizationId: organizationId,
                        employeeId: employee.RowID,
                        leaveLedgerId: vacationLeaveLedger.RowID,
                        payPeriodId: payPeriod.RowID,
                        paystubId: null,
                        referenceId: null,
                        transactionDate: payPeriod.PayFromDate,
                        description: string.Empty,
                        type: LeaveTransactionType.Credit,
                        amount: update.VacationLeaveCredit,
                        balance: update.VacationLeaveCredit);

                    _context.LeaveTransactions.Add(newVacationLeaveTransaction);
                    vacationLeaveLedger.LastTransaction = newVacationLeaveTransaction;
                    _context.Entry(vacationLeaveLedger).State = EntityState.Modified;

                    // sick leave
                    employee.SickLeaveAllowance = update.SickLeaveCredit;
                    employee.SickLeaveBalance = update.SickLeaveCredit;

                    var sickLeaveLedger = leaveLedgers.Where(l => l.EmployeeID == employee.RowID)
                        .Where(l => l.Product.PartNo == ProductConstant.SICK_LEAVE)
                        .FirstOrDefault();

                    var newSickLeaveTransaction = LeaveTransaction.NewLeaveTransaction(userId: userId,
                        organizationId: organizationId,
                        employeeId: employee.RowID,
                        leaveLedgerId: sickLeaveLedger.RowID,
                        payPeriodId: payPeriod.RowID,
                        paystubId: null,
                        referenceId: null,
                        transactionDate: payPeriod.PayFromDate,
                        description: string.Empty,
                        type: LeaveTransactionType.Credit,
                        amount: update.SickLeaveCredit,
                        balance: update.SickLeaveCredit);

                    _context.LeaveTransactions.Add(newSickLeaveTransaction);
                    sickLeaveLedger.LastTransaction = newSickLeaveTransaction;
                    _context.Entry(sickLeaveLedger).State = EntityState.Modified;

                    employee.LastUpdBy = userId;

                    _context.Entry(employee).State = EntityState.Modified;

                }

                i.Update(userId: userId,
                    vacationCredit: update.VacationLeaveCredit,
                    sickCredit: update.SickLeaveCredit,
                    isSelected: update.IsSelected,
                    isApplied: true);

                _context.Entry(i).State = EntityState.Modified;
            }
            
            await _context.SaveChangesAsync();
        }

        public async Task SaveManyAsync2(int organizationId,
            int userId,
            IReadOnlyCollection<ResetLeaveCreditModel> resetLeaveCreditModels)
        {
            var newItems = resetLeaveCreditModels
                .Where(t => t.IsNew)
                .Select(t => {
                    var resetLeaveCredit = t.OriginalResetLeaveCredit;

                    resetLeaveCredit.PayPeriod = null;

                    var items = t.Items.Select(i => {
                        var resetLeaveCreditItem = i.OriginalResetLeaveCreditItem;

                        resetLeaveCreditItem.Update(userId: userId,
                            vacationCredit: i.VacationLeaveCredit,
                            sickCredit: i.SickLeaveCredit,
                            isSelected: i.IsSelected,
                            isApplied: i.IsApplied);

                        resetLeaveCreditItem.Employee = null;
                        resetLeaveCreditItem.ResetLeaveCredit = null;

                        return resetLeaveCreditItem;
                    }).ToList();
                    resetLeaveCredit.ResetLeaveCreditItems = items;
                    return resetLeaveCredit;
                })
                .ToList();

            if(newItems != null && newItems.Any()) await SaveManyAsync(newItems, userId);

            var updateItems = resetLeaveCreditModels
                .Where(t => !t.IsNew && t.HasChanged)
                .Select(t => {
                    var resetLeaveCredit = t.OriginalResetLeaveCredit;

                    resetLeaveCredit.PayPeriod = null;

                    var items = t.Items.Select(i => {
                        var resetLeaveCreditItem = i.OriginalResetLeaveCreditItem;

                        resetLeaveCreditItem.Update(userId: userId,
                            vacationCredit: i.VacationLeaveCredit,
                            sickCredit: i.SickLeaveCredit,
                            isSelected: i.IsSelected,
                            isApplied: i.IsApplied);

                        resetLeaveCreditItem.Employee = null;
                        resetLeaveCreditItem.ResetLeaveCredit = null;

                        return resetLeaveCreditItem;
                    }).ToList();
                    resetLeaveCredit.ResetLeaveCreditItems = items;
                    return resetLeaveCredit;
                })
                .ToList();

            if(updateItems != null && updateItems.Any()) await SaveManyAsync(updateItems, userId);

        }

        protected override string CreateUserActivitySuffixIdentifier(ResetLeaveCredit entity) => $" with '{entity.ResetLeaveCreditItems.Where(t => t.IsSelected).Count()} selected employee(s), and with {entity.ResetLeaveCreditItems.Where(t => t.VacationLeaveCredit > 0 || t.SickLeaveCredit > 0).Count()} corresponding vacation & sick leave beginning credit";

        protected override string GetUserActivityName(ResetLeaveCredit entity) => UserActivityResetLeaveCreditName;

        protected override async Task RecordUpdate(ResetLeaveCredit updatedEntity, ResetLeaveCredit oldEntity)
        {
            var changes = new List<UserActivityItem>();
            var entityName = UserActivityResetLeaveCreditName.ToLower();

            oldEntity.ResetLeaveCreditItems.ToList().ForEach(old =>
            {
                var updated = updatedEntity.ResetLeaveCreditItems
                    .FirstOrDefault(u => u.RowID == old.RowID);
                if (updated == null) return;

                if (old.IsSelected != updated.IsSelected)
                    changes.Add(new UserActivityItem()
                    {
                        EntityId = old.RowID.Value,
                        Description = $"Updated IsSelected from '{old.IsSelected}' to '{updated.IsSelected}'",
                        ChangedEmployeeId = old.EmployeeID.Value
                    });
                if (old.VacationLeaveCredit != updated.VacationLeaveCredit)
                    changes.Add(new UserActivityItem()
                    {
                        EntityId = old.RowID.Value,
                        Description = $"Updated VacationLeaveCredit from '{old.VacationLeaveCredit.ToString("#,##0.00")}' to '{updated.VacationLeaveCredit.ToString("#,##0.00")}'",
                        ChangedEmployeeId = old.EmployeeID.Value
                    });
                if (old.SickLeaveCredit != updated.SickLeaveCredit)
                    changes.Add(new UserActivityItem()
                    {
                        EntityId = old.RowID.Value,
                        Description = $"Updated SickLeaveCredit from '{old.SickLeaveCredit.ToString("#,##0.00")}' to '{updated.SickLeaveCredit.ToString("#,##0.00")}'",
                        ChangedEmployeeId = old.EmployeeID.Value
                    });
            });

            if (changes.Any())
                await _userActivityRepository.CreateRecordAsync(
                        updatedEntity.LastUpdBy.Value,
                        UserActivityResetLeaveCreditName,
                        updatedEntity.OrganizationID.Value,
                        UserActivity.RecordTypeEdit,
                        changes);
        }
    }
}
