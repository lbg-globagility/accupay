using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Core.ValueObjects;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AccuPay.Core.Entities.Paystub;

namespace AccuPay.Infrastructure.Data
{
    public class PaystubRepository : BaseRepository, IPaystubRepository
    {
        private readonly PayrollContext _context;

        public PaystubRepository(PayrollContext context)
        {
            _context = context;
        }

        #region Save

        public async Task DeleteAsync(int id, int currentlyLoggedInUserId)
        {
            await DeleteAsyncWithContext(id: id, userId: currentlyLoggedInUserId);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyCollection<Paystub>> DeleteByPeriodAsync(int payPeriodId, int currentlyLoggedInUserId)
        {
            var payStubs = await _context.Paystubs
                .Where(x => x.PayPeriodID == payPeriodId)
                .ToListAsync();

            var payStubIds = payStubs.Select(x => x.RowID.Value);

            foreach (int id in payStubIds)
            {
                await DeleteAsyncWithContext(id: id, userId: currentlyLoggedInUserId);
            }

            await _context.SaveChangesAsync();

            return payStubs;
        }

        public async Task UpdateManyThirteenthMonthPaysAsync(ICollection<ThirteenthMonthPay> thirteenthMonthPays)
        {
            foreach (var thirteenthMonthPay in thirteenthMonthPays)
            {
                _context.Entry(thirteenthMonthPay).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
        }

        #region Create or Update Paystub

        public async Task SaveAsync(
            int currentlyLoggedInUserId,
            string currentSystemOwner,
            IPolicyHelper policy,
            PayPeriod payPeriod,
            Paystub paystub,
            Employee employee,
            Product bpiInsuranceProduct,
            Product sickLeaveProduct,
            Product vacationLeaveProduct,
            Product singleParentLeaveProduct,
            IReadOnlyCollection<Loan> loans,
            ICollection<AllowanceItem> allowanceItems,
            ICollection<LoanTransaction> loanTransactions,
            IReadOnlyCollection<TimeEntry> timeEntries,
            IReadOnlyCollection<Leave> leaves,
            IReadOnlyCollection<Bonus> bonuses)
        {
            foreach (var loan in loans)
            {
                SaveLoanPaymentFromBonusItems(
                    payPeriod: payPeriod,
                    paystub: paystub,
                    loan: loan,
                    bonuses: bonuses,
                    useLoanDeductFromBonus: policy.UseLoanDeductFromBonus);

                SaveYearlyLoanInterest(policy, loan);

                SaveLoan(currentlyLoggedInUserId, loan);
            }

            SavePaystub(currentlyLoggedInUserId, paystub);

            SaveBPIInsurance(currentlyLoggedInUserId, policy, payPeriod, paystub, employee, bpiInsuranceProduct);

            SaveAllowanceItems(paystub, allowanceItems);

            SaveLoanTransactions(paystub, loanTransactions);

            await UpdateLeaveLedgerAndPaystubItems(currentlyLoggedInUserId, currentSystemOwner, payPeriod, paystub, employee, sickLeaveProduct, vacationLeaveProduct, singleParentLeaveProduct: singleParentLeaveProduct, timeEntries, leaves);

            DetachInvalidEntities();

            await _context.SaveChangesAsync();
        }

        private async Task UpdateLeaveLedgerAndPaystubItems(int currentlyLoggedInUserId, string currentSystemOwner, PayPeriod payPeriod, Paystub paystub, Employee employee, Product sickLeaveProduct, Product vacationLeaveProduct, Product singleParentLeaveProduct, IReadOnlyCollection<TimeEntry> timeEntries, IReadOnlyCollection<Leave> leaves)
        {
            if (currentSystemOwner != SystemOwner.Benchmark)
            {
                await UpdateLeaveLedger(paystub, employee, payPeriod, timeEntries, leaves);

                await UpdatePaystubItems(
                    currentlyLoggedInUserId,
                    paystub,
                    employee,
                    sickLeaveProduct: sickLeaveProduct,
                    vacationLeaveProduct: vacationLeaveProduct,
                    singleParentLeaveProduct: singleParentLeaveProduct,
                    timeEntries);
            }
            else
            {
                await UpdateBenchmarkLeaveLedger(paystub, employee, payPeriod);
            }
        }

        private void SaveLoanTransactions(Paystub paystub, ICollection<LoanTransaction> loanTransactions)
        {
            if (paystub.LoanTransactions != null)
            {
                _context.LoanTransactions.RemoveRange(paystub.LoanTransactions);
            }
            paystub.LoanTransactions = loanTransactions;
        }

        private void SaveAllowanceItems(Paystub paystub, ICollection<AllowanceItem> allowanceItems)
        {
            if (paystub.AllowanceItems != null)
            {
                _context.AllowanceItems.RemoveRange(paystub.AllowanceItems);
            }
            paystub.AllowanceItems = allowanceItems;
        }

        private void SaveBPIInsurance(int currentlyLoggedInUserId, IPolicyHelper policy, PayPeriod payPeriod, Paystub paystub, Employee employee, Product bpiInsuranceProduct)
        {
            bool isEligibleForBPIInsurance = employee.IsEligibleForNewBPIInsurance(
                useBPIInsurancePolicy: policy.UseBPIInsurance,
                paystub.Adjustments,
                payPeriod);

            if (isEligibleForBPIInsurance)
            {
                _context.Adjustments.Add(new Adjustment()
                {
                    OrganizationID = paystub.OrganizationID,
                    CreatedBy = currentlyLoggedInUserId,
                    Paystub = paystub,
                    ProductID = bpiInsuranceProduct.RowID,
                    Amount = -employee.BPIInsurance
                });
            }
        }

        private void SavePaystub(int currentlyLoggedInUserId, Paystub paystub)
        {
            if (paystub.IsNewEntity)
            {
                _context.Paystubs.Add(paystub);
            }
            else
            {
                paystub.LastUpdBy = currentlyLoggedInUserId;
                _context.Entry(paystub).State = EntityState.Modified;
                _context.Entry(paystub.Actual).State = EntityState.Modified;

                if (paystub.ThirteenthMonthPay != null)
                {
                    _context.Entry(paystub.ThirteenthMonthPay).State = EntityState.Modified;
                }
            }
        }

        private void DetachInvalidEntities()
        {
            var invalidEntities = new List<object>();
            foreach (var e in _context.ChangeTracker.Entries())
            {
                string fullEntityName = e.Entity.GetType().FullName;
                Console.WriteLine("Name and Status: {0} is {1}", fullEntityName, e.State);

                // add more entities here if they are not supposed to be saved
                if (fullEntityName.Contains(nameof(Employee)) ||
                    fullEntityName.Contains(nameof(Division)) ||
                    fullEntityName.Contains(nameof(Position)))
                {
                    invalidEntities.Add(e.Entity);
                }
            }

            foreach (var entity in invalidEntities)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            foreach (var e in _context.ChangeTracker.Entries())
            {
                Console.WriteLine("### Name and Status: {0} is {1}", e.Entity.GetType().FullName, e.State);
            }
        }

        private void SaveLoan(int currentlyLoggedInUserId, Loan loan)
        {
            loan.LastUpdBy = currentlyLoggedInUserId;
            _context.Entry(loan).State = EntityState.Modified;
        }

        private void SaveYearlyLoanInterest(IPolicyHelper policy, Loan loan)
        {
            if (policy.UseGoldwingsLoanInterest && loan.YearlyLoanInterests != null && loan.YearlyLoanInterests.Any())
            {
                loan.YearlyLoanInterests.ToList().ForEach(yearlyLoanInterest =>
                {
                    // Save new yearly loan interest.
                    // Don't save updated yearlyLoanInterests since they should not be edited after it was created.
                    if (yearlyLoanInterest.IsNewEntity)
                    {
                        _context.Entry(yearlyLoanInterest).State = EntityState.Added;
                    }
                });
            }
        }

        private void SaveLoanPaymentFromBonusItems(
            PayPeriod payPeriod,
            Paystub paystub,
            Loan loan,
            IReadOnlyCollection<Bonus> bonuses,
            bool useLoanDeductFromBonus)
        {
            if (useLoanDeductFromBonus)
            {
                var loanPaymentFromBonuses = paystub.GetLoanPaymentFromBonuses(bonuses, loan, payPeriod: payPeriod);

                foreach (var lb in loanPaymentFromBonuses)
                {
                    var existingItem = lb.Items
                        .Where(i => i.LoanPaidBonusId == lb.Id)
                        .Where(i => i.PaystubId == paystub.RowID)
                        .FirstOrDefault();

                    if (existingItem != null) continue;

                    var lbItem = LoanPaymentFromBonusItem.CreateNew(lb.Id, paystub);

                    lb.Items.Add(lbItem);

                    _context.Entry(lbItem).State = EntityState.Added;
                }
            }
        }

        private async Task UpdateBenchmarkLeaveLedger(Paystub paystub, Employee employee, PayPeriod payPeriod)
        {
            var vacationLedger = await _context.LeaveLedgers
                .AsNoTracking()
                .Include(l => l.Product)
                .Include(l => l.LastTransaction)
                .Where(l => l.EmployeeID == employee.RowID)
                .Where(l => l.Product.IsVacationLeave)
                .FirstOrDefaultAsync();

            vacationLedger.LeaveTransactions = new List<LeaveTransaction>();

            if (vacationLedger == null)
                throw new Exception($"Vacation ledger for Employee No.: {employee.EmployeeNo}");

            UpdateLedgerTransaction(
                paystub: paystub,
                payPeriod: payPeriod,
                leaveId: null, ledger:
                vacationLedger, totalLeaveHours:
                paystub.LeaveHours,
                transactionDate: payPeriod.PayToDate);
        }

        private async Task UpdateLeaveLedger(
            Paystub paystub,
            Employee employee,
            PayPeriod payPeriod,
            IReadOnlyCollection<TimeEntry> timeEntries,
            IReadOnlyCollection<Leave> leaves)
        {
            // use LeaveLedgerRepository
            var employeeLeaves = leaves
                .Where(l => l.EmployeeID == employee.RowID)
                .OrderBy(l => l.StartDate)
                .ToList();

            var leaveIds = employeeLeaves.Select(l => l.RowID).ToArray();

            List<LeaveTransaction> transactions = new List<LeaveTransaction>() { };
            if (leaveIds.Any())
            {
                transactions =
                    await (from t in _context.LeaveTransactions
                           where leaveIds.Contains(t.ReferenceID)
                           select t)
                           .AsNoTracking()
                           .ToListAsync();
            }

            var employeeId = employee.RowID;
            var ledgers = await _context.LeaveLedgers
                //.AsNoTracking()
                .Include(x => x.Product)
                .Include(x => x.LeaveTransactions)
                .Include(x => x.LastTransaction)
                .Where(x => x.EmployeeID == employeeId)
                .ToListAsync();

            var newLeaveTransactions = new List<LeaveTransaction>();
            foreach (var leave in employeeLeaves)
            {
                // If a transaction has already been made for the current leave, skip the current leave.
                if (transactions.Any(t => t.ReferenceID == leave.RowID))
                {
                    continue;
                }
                else
                {
                    var ledger = ledgers.FirstOrDefault(l => l.Product.PartNo == leave.LeaveType);

                    // retrieves the time entries within leave date range
                    var timeEntry = timeEntries?.Where(t => leave.StartDate == t.Date);

                    if (timeEntry == null)
                        continue;

                    // summate the leave hours
                    var totalLeaveHours = timeEntry.Sum(t => t.TotalLeaveHours);

                    if (totalLeaveHours == 0)
                        continue;

                    var transactionDate = leave.EndDate ?? leave.StartDate;

                    UpdateLedgerTransaction(
                        paystub: paystub,
                        payPeriod: payPeriod,
                        leaveId: leave.RowID,
                        ledger: ledger,
                        totalLeaveHours: totalLeaveHours,
                        transactionDate: transactionDate);
                }
            }
        }

        private void UpdateLedgerTransaction(
            Paystub paystub,
            PayPeriod payPeriod,
            int? leaveId,
            LeaveLedger ledger,
            decimal totalLeaveHours,
            DateTime transactionDate)
        {
            //var newTransaction = new LeaveTransaction()
            //{
            //    OrganizationID = paystub.OrganizationID,
            //    Created = DateTime.Now,
            //    EmployeeID = paystub.EmployeeID,
            //    PayPeriodID = payPeriod.RowID,
            //    ReferenceID = leaveId,
            //    TransactionDate = transactionDate,
            //    Type = LeaveTransactionType.Debit,
            //    Amount = totalLeaveHours,
            //    Paystub = paystub,
            //    Balance = (ledger?.LastTransaction?.Balance ?? 0) - totalLeaveHours
            //};
            var newTransaction = LeaveTransaction.NewLeaveTransaction(leaveLedgerId: null,
                employeeId: paystub.EmployeeID,
                userId: null,
                organizationId: paystub.OrganizationID,
                type: LeaveTransactionType.Debit,
                transactionDate: transactionDate,
                amount: totalLeaveHours,
                description: string.Empty,
                balance: (ledger?.LastTransaction?.Balance ?? 0) - totalLeaveHours,
                payPeriodId: payPeriod.RowID,
                paystubId: null,
                referenceId: leaveId);
            newTransaction.Paystub = paystub;

            ledger.LeaveTransactions.Add(newTransaction);
            ledger.LastTransaction = newTransaction;
        }

        private async Task UpdatePaystubItems(
            int currentlyLoggedInUserId,
            Paystub paystub,
            Employee employee,
            Product sickLeaveProduct,
            Product vacationLeaveProduct,
            Product singleParentLeaveProduct,
            IReadOnlyCollection<TimeEntry> timeEntries)
        {
            _context.Entry(paystub).Collection(p => p.PaystubItems).Load();
            _context.Set<PaystubItem>().RemoveRange(paystub.PaystubItems);

            var vacationLeaveBalance = await _context.PaystubItems
                .AsNoTracking()
                .Where(p => p.Product.PartNo == ProductConstant.VACATION_LEAVE)
                .Where(p => p.Paystub.RowID == paystub.RowID)
                .FirstOrDefaultAsync();

            decimal vacationLeaveUsed = timeEntries?.Sum(t => t.VacationLeaveHours) ?? 0;
            decimal newBalance = employee.LeaveBalance - vacationLeaveUsed;

            vacationLeaveBalance = new PaystubItem()
            {
                OrganizationID = paystub.OrganizationID,
                Created = DateTime.Now,
                CreatedBy = currentlyLoggedInUserId,
                ProductID = vacationLeaveProduct.RowID,
                PayAmount = newBalance,
                Paystub = paystub
            };

            paystub.PaystubItems.Add(vacationLeaveBalance);

            var sickLeaveBalance = await _context.PaystubItems
                .AsNoTracking()
                .Where(p => p.Product.PartNo == ProductConstant.SICK_LEAVE)
                .Where(p => p.Paystub.RowID == paystub.RowID)
                .FirstOrDefaultAsync();

            var sickLeaveUsed = timeEntries?.Sum(t => t.SickLeaveHours) ?? 0;
            var newBalance2 = employee.SickLeaveBalance - sickLeaveUsed;

            sickLeaveBalance = new PaystubItem()
            {
                OrganizationID = paystub.OrganizationID,
                Created = DateTime.Now,
                CreatedBy = currentlyLoggedInUserId,
                ProductID = sickLeaveProduct.RowID,
                PayAmount = newBalance2,
                Paystub = paystub
            };

            paystub.PaystubItems.Add(sickLeaveBalance);

            var singleParentLeaveBalance = await _context.PaystubItems
                .AsNoTracking()
                .Where(p => p.Product.PartNo == ProductConstant.SINGLE_PARENT_LEAVE)
                .Where(p => p.Paystub.RowID == paystub.RowID)
                .FirstOrDefaultAsync();

            decimal singleParentLeaveUsed = timeEntries?.Sum(t => t.SingleParentLeaveHours) ?? 0;
            decimal newBalance3 = employee.LeaveBalance - vacationLeaveUsed;

            singleParentLeaveBalance = new PaystubItem()
            {
                OrganizationID = paystub.OrganizationID,
                Created = DateTime.Now,
                CreatedBy = currentlyLoggedInUserId,
                ProductID = singleParentLeaveProduct.RowID,
                PayAmount = newBalance3,
                Paystub = paystub
            };

            paystub.PaystubItems.Add(singleParentLeaveBalance);
        }

        #endregion Create or Update Paystub

        #region UpdateAdjustments

        public async Task<(IReadOnlyCollection<T> added, IReadOnlyCollection<T> updated, IReadOnlyCollection<T> deleted, IReadOnlyCollection<T> originalAdjustments)> UpdateAdjustmentsAsync<T>(
            int paystubId,
            ICollection<T> allAdjustments) where T : IAdjustment
        {
            Paystub paystub = await GetPaystubWithAdjustments(paystubId);

            decimal originalDeclaredAdjustmentAmount = paystub.Adjustments.Sum(x => x.Amount);
            decimal originalActualAdjustmentAmount = paystub.ActualAdjustments.Sum(x => x.Amount);
            decimal originalTotalAdjustmentAmount = originalDeclaredAdjustmentAmount + originalActualAdjustmentAmount;

            // for saving
            List<T> currentAdjustments = GetCurrentAdjustments<T>(paystub);

            // for user activity
            List<T> originalAdjustments = new List<T>();
            currentAdjustments.ForEach(x =>
            {
                originalAdjustments.Add((T)x.Clone());
            });

            List<T> modifiedAdjustments = GetModifiedAdjustments(allAdjustments);

            (IReadOnlyCollection<T> added, IReadOnlyCollection<T> updated, IReadOnlyCollection<T> deleted) =
                GetAdjustmentModifications(paystub, currentAdjustments, modifiedAdjustments);

            AddContextState(added, EntityState.Added);
            AddContextState(updated, EntityState.Modified);
            AddContextState(deleted, EntityState.Deleted);

            // Save changes
            decimal updatedTotalAdjustments = paystub.Adjustments.Sum(x => x.Amount) + paystub.ActualAdjustments.Sum(x => x.Amount);

            if (typeof(Adjustment).IsAssignableFrom(typeof(T)))
            {
                paystub.TotalAdjustments = paystub.Adjustments.Sum(x => x.Amount);
                paystub.NetPay = (paystub.NetPay - originalDeclaredAdjustmentAmount) + paystub.TotalAdjustments;
            }

            paystub.Actual.TotalAdjustments = updatedTotalAdjustments;
            paystub.Actual.NetPay = (paystub.Actual.NetPay - originalTotalAdjustmentAmount) + paystub.Actual.TotalAdjustments;

            _context.Entry(paystub).State = EntityState.Modified;
            _context.Entry(paystub.Actual).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return (
                added: added,
                updated: updated,
                deleted: deleted,
                originalAdjustments: originalAdjustments);
        }

        private void AddContextState<T>(IReadOnlyCollection<T> adjustments, EntityState entityState) where T : IAdjustment
        {
            if (adjustments == null || !adjustments.Any()) return;

            foreach (var adjustment in adjustments)
            {
                _context.Entry(adjustment).State = entityState;
            }
        }

        private static List<T> GetCurrentAdjustments<T>(Paystub paystub) where T : IAdjustment
        {
            var originalAdjustments = new List<T>();
            if (typeof(ActualAdjustment).IsAssignableFrom(typeof(T)))
            {
                paystub.ActualAdjustments
                    .Where(x => x.IsActual)
                    .ToList()
                    .ForEach((a) =>
                    {
                        originalAdjustments.Add((T)(object)a);
                    });
            }
            else
            {
                paystub.Adjustments
                    .Where(x => !x.IsActual)
                    .ToList()
                    .ForEach((a) =>
                    {
                        originalAdjustments.Add((T)(object)a);
                    });
            }

            return originalAdjustments;
        }

        private static List<T> GetModifiedAdjustments<T>(ICollection<T> allAdjustments) where T : IAdjustment
        {
            var modifiedAdjustments = new List<T>();

            allAdjustments
                .Where(x =>
                {
                    if (typeof(ActualAdjustment).IsAssignableFrom(typeof(T)))
                    {
                        return x.IsActual;
                    }
                    else
                    {
                        return !x.IsActual;
                    }
                })
                .ToList()
                .ForEach((a) =>
                {
                    modifiedAdjustments.Add(a);
                });
            return modifiedAdjustments;
        }

        public async Task<ICollection<Paystub>> GetByPaystubsForLoanPaymentFrom13thMonthAsync(int payPeriodId)
        {
            return await CreateBaseQueryWithFullPaystub()
                .Include(x => x.LoanPaymentFromThirteenthMonthPays)
                .Where(x => x.PayPeriodID == payPeriodId)
                .Where(x => x.Adjustments.Any(a => a.Is13thMonthPay))
                //.Where(x => x.ActualAdjustments.Any(a => a.Is13thMonthPay))
                .ToListAsync();
        }

        private (IReadOnlyCollection<T> added, IReadOnlyCollection<T> updated, IReadOnlyCollection<T> deleted) GetAdjustmentModifications<T>(
            Paystub paystub,
            List<T> originalAdjustments,
            List<T> modifiedAdjustments) where T : IAdjustment
        {
            List<T> added = new List<T>();
            List<T> updated = new List<T>();
            List<T> deleted = new List<T>();

            List<T> newAdjustments = modifiedAdjustments.Where(x => IsNewEntity(x.RowID)).ToList();
            List<T> updatedAdjustments = modifiedAdjustments.Where(x => !IsNewEntity(x.RowID)).ToList();

            foreach (T adjustment in originalAdjustments)
            {
                T updatedAdjustment = updatedAdjustments
                    .Where(x => x.RowID == adjustment.RowID)
                    .FirstOrDefault();

                if (updatedAdjustment == null)
                {
                    // removing adjustment from paystub.Adjustments is not needed for entity tracking
                    // but since we use the sum of paystub.Adjustments, we need to update the collection
                    if (typeof(Adjustment).IsAssignableFrom(typeof(T)))
                    {
                        paystub.Adjustments.Remove((Adjustment)(object)adjustment);
                    }
                    else
                    {
                        paystub.ActualAdjustments.Remove((ActualAdjustment)(object)adjustment);
                    }

                    deleted.Add(adjustment);
                }
                else
                {
                    adjustment.Amount = updatedAdjustment.Amount;
                    adjustment.ProductID = updatedAdjustment.ProductID;
                    adjustment.Comment = updatedAdjustment.Comment;
                    adjustment.Is13thMonthPay = updatedAdjustment.Is13thMonthPay;

                    updated.Add(adjustment);
                }
            }

            foreach (var newAdjustment in newAdjustments)
            {
                // adding adjustment from paystub.Adjustments is not needed for entity tracking
                // but since we use the sum of paystub.Adjustments, we need to update the collection
                if (typeof(Adjustment).IsAssignableFrom(typeof(T)))
                {
                    paystub.Adjustments.Add((Adjustment)(object)newAdjustment);
                }
                else
                {
                    paystub.ActualAdjustments.Add((ActualAdjustment)(object)newAdjustment);
                }

                added.Add(newAdjustment);
            }

            return (
                added: added,
                updated: updated,
                deleted: deleted);
        }

        #endregion UpdateAdjustments

        private async Task DeleteAsyncWithContext(int id, int userId)
        {
            var paystub = await _context.Paystubs
                .Include(x => x.LoanTransactions)
                    .ThenInclude(x => x.Loan)
                .Include(x => x.LeaveTransactions)
                    .ThenInclude(x => x.LeaveLedger)

                .Include(x => x.Adjustments)
                .Include(x => x.ActualAdjustments)
                .Include(x => x.PaystubItems)
                .Include(x => x.PaystubEmails)
                .Include(x => x.PaystubEmailHistories)

                .Include(x => x.ThirteenthMonthPay)
                .Include(x => x.Actual)
                .Where(x => x.RowID == id)
                .FirstOrDefaultAsync();

            // Some of this are already deleted cascadingly if we delete the paystub but the
            // aggregate root should handle the delete of of all the entities in its aggregate.
            // AND some database from other clients have cascade constraint, some have not.

            // Deleting of paystub bonus is not yet supported. TODO and test it

            ResetLoanBalances(paystub.LoanTransactions, userId);
            _context.LoanTransactions.RemoveRange(paystub.LoanTransactions);

            // RemoveRange does not delete the entities immediately so the order of
            // resetting the parent entity and deleting that child are not important
            ResetLeaveLedgerTransactions(paystub.LeaveTransactions, userId);
            _context.LeaveTransactions.RemoveRange(paystub.LeaveTransactions);

            _context.AllowanceItems.RemoveRange(paystub.AllowanceItems);
            _context.Adjustments.RemoveRange(paystub.Adjustments);
            _context.ActualAdjustments.RemoveRange(paystub.ActualAdjustments);
            _context.PaystubEmails.RemoveRange(paystub.PaystubEmails);
            _context.PaystubEmailHistories.RemoveRange(paystub.PaystubEmailHistories);
            _context.PaystubItems.RemoveRange(paystub.PaystubItems);

            _context.ThirteenthMonthPays.Remove(paystub.ThirteenthMonthPay);
            _context.PaystubActuals.Remove(paystub.Actual);

            _context.Paystubs.Remove(paystub);
        }

        #endregion Save

        #region Queries

        #region Child data

        public async Task<ICollection<AllowanceItem>> GetAllowanceItemsAsync(int paystubId)
        {
            var paystub = await _context.Paystubs
                .Include(x => x.AllowanceItems)
                    .ThenInclude(x => x.Allowance)
                        .ThenInclude(x => x.Product)
                .Include(x => x.AllowanceItems)
                    .ThenInclude(x => x.AllowancesPerDay)
                .Where(x => x.RowID == paystubId)
                .FirstOrDefaultAsync();

            return paystub.AllowanceItems;
        }

        public async Task<ICollection<Adjustment>> GetAdjustmentsAsync(int paystubId)
        {
            var paystub = await BaseGetAdjustments(paystubId)
                .FirstOrDefaultAsync();

            return paystub.Adjustments;
        }

        private IQueryable<Paystub> BaseGetAdjustments(int paystubId)
        {
            return _context.Paystubs
                .AsNoTracking()
                .Include(x => x.Adjustments)
                    .ThenInclude(x => x.Product)
                .Where(x => x.RowID == paystubId);
        }

        public async Task<ICollection<ActualAdjustment>> GetActualAdjustmentsAsync(int paystubId)
        {
            var paystub = await _context.Paystubs
                .AsNoTracking()
                .Include(x => x.ActualAdjustments)
                    .ThenInclude(x => x.Product)
                .Where(x => x.RowID == paystubId)
                .FirstOrDefaultAsync();

            return paystub.ActualAdjustments;
        }

        public async Task<ICollection<LoanTransaction>> GetLoanTransactionsAsync(int paystubId)
        {
            var paystub = await _context.Paystubs
                .Include(x => x.LoanTransactions)
                    .ThenInclude(x => x.Loan)
                        .ThenInclude(x => x.LoanType)
                .Where(x => x.RowID == paystubId)
                .FirstOrDefaultAsync();

            return paystub.LoanTransactions;
        }

        public async Task<Paystub> GetPaystubWithAdjustments(int paystubId)
        {
            return await _context.Paystubs
                .AsNoTracking()
                .Include(x => x.Actual)
                .Include(x => x.Adjustments)
                .Include(x => x.ActualAdjustments)
                .Where(x => x.RowID == paystubId)
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<Paystub>> GetPaystubWithAdjustmentsByEmployeeAndDatePeriodAsync(
            int organizationId,
            int[] employeeIds,
            TimePeriod timePeriod,
            bool includeActual = true)
        {
            var query = _context.Paystubs
                .AsNoTracking()
                .Include(x => x.Actual)
                .Include(x => x.Adjustments)
                    .ThenInclude(a => a.Product)
                .Include(x => x.PayPeriod)
                .AsQueryable();

            if (includeActual)
            {
                query = query
                    .Include(x => x.ActualAdjustments)
                        .ThenInclude(a => a.Product);
            }

            return await query
                .Where(x => x.OrganizationID == organizationId)
                .Where(x => x.PayPeriod.PayFromDate >= timePeriod.Start)
                .Where(x => x.PayPeriod.PayToDate <= timePeriod.End)
                .Where(x => employeeIds.Contains(x.EmployeeID.Value))
                .ToListAsync();
        }

        #endregion Child data

        public async Task<Paystub> GetByIdAsync(int id)
        {
            return await _context.Paystubs
                .Include(x => x.ThirteenthMonthPay)
                .Include(x => x.Actual)
                .Where(x => x.RowID == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Paystub> GetByCompositeKeyAsync(EmployeeCompositeKey key)
        {
            var query = _context.Paystubs.AsNoTracking();

            query = AddGetByEmployeeCompositeKeyQuery(key, query);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Paystub> GetByCompositeKeyWithActualAndThirteenthMonthAsync(EmployeeCompositeKey key)
        {
            var query = _context.Paystubs
                .AsNoTracking()
                .Include(x => x.ThirteenthMonthPay)
                .Include(x => x.Actual)
                .AsQueryable();

            query = AddGetByEmployeeCompositeKeyQuery(key, query);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Paystub> GetByCompositeKeyFullPaystubAsync(EmployeeCompositeKey key)
        {
            var query = CreateBaseQueryWithFullPaystub();
            query = AddGetByEmployeeCompositeKeyQuery(key, query);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<ICollection<Paystub>> GetByPayPeriodFullPaystubAsync(int payPeriodId)
        {
            return await CreateBaseQueryWithFullPaystub()
                .Where(x => x.PayPeriodID == payPeriodId)
                .ToListAsync();
        }

        public async Task<ICollection<Paystub>> GetByTimePeriodWithThirteenthMonthPayAndEmployeeAsync(TimePeriod timePeriod, int organizationId)
        {
            return await _context.Paystubs
                .Include(x => x.PayPeriod)
                .Include(x => x.ThirteenthMonthPay)
                .Include(x => x.Employee)
                .Where(x => x.OrganizationID == organizationId)
                .Where(x => x.PayPeriod.PayFromDate >= timePeriod.Start)
                .Where(x => x.PayPeriod.PayToDate <= timePeriod.End)
                .ToListAsync();
        }

        public async Task<ICollection<Paystub>> GetPreviousCutOffPaystubsAsync(
            DateTime currentCuttOffStart,
            int organizationId)
        {
            var previousCutoffEnd = currentCuttOffStart.AddDays(-1);

            return await _context.Paystubs
                .AsNoTracking()
                .Where(x => x.PayToDate == previousCutoffEnd)
                .Where(x => x.OrganizationID == organizationId)
                .ToListAsync();
        }

        public async Task<ICollection<Paystub>> GetAllWithEmployeeAsync(DateCompositeKey key)
        {
            return await _context.Paystubs
                .Include(x => x.Employee)
                .Include(x => x.Actual)
                .Where(x => x.PayFromDate == key.PayFromDate)
                .Where(x => x.PayToDate == key.PayToDate)
                .Where(x => x.OrganizationID == key.OrganizationId)
                .ToListAsync();
        }

        public async Task<ICollection<Paystub>> GetByPayPeriodWithEmployeeAsync(int payPeriodId) => await _context.Paystubs
            .AsNoTracking()
            .Include(x => x.Employee)
            .Where(x => x.PayPeriodID == payPeriodId)
            .ToListAsync();

        public async Task<ICollection<Paystub>> GetByPayPeriodWithEmployeesOfSamePayFrequencyAsync(int payPeriodId)
        {
            var payPeriod = await _context.PayPeriods
                .AsNoTracking()
                .Include(p => p.Organization)
                    .ThenInclude(o => o.PayFrequency)
                .FirstOrDefaultAsync(p => p.RowID == payPeriodId);
            var originOrganization = payPeriod.Organization;
            var organizationsOfSamePayFrequency = Enumerable.Empty<Organization>();
            if (originOrganization.IsWeekly)
            {
                organizationsOfSamePayFrequency = await _context.Organizations
                    .AsNoTracking()
                    .Where(o => o.PayFrequencyID == payPeriod.PayFrequencyID)
                    .Where(o => o.IsAtm == originOrganization.IsAtm)
                    .Where(o => o.IsCash == originOrganization.IsCash)
                    .ToListAsync();
            }
            else
            {
                organizationsOfSamePayFrequency = await _context.Organizations
                    .AsNoTracking()
                    .Where(o => o.PayFrequencyID == payPeriod.PayFrequencyID)
                    .ToListAsync();
            }
            var organizationIds = organizationsOfSamePayFrequency.Select(o => o.RowID.Value)
                .ToArray();

            var payPeriods = await _context.PayPeriods
                .AsNoTracking()
                .Where(p => organizationIds.Contains(p.OrganizationID.Value))
                .Where(p => p.Month == payPeriod.Month)
                .Where(p => p.Year == payPeriod.Year)
                .Where(p => p.OrdinalValue == payPeriod.OrdinalValue)
                .ToListAsync();
            var payPeriodIds = payPeriods.Select(p => p.RowID.Value).ToArray();

            return await _context.Paystubs
                .AsNoTracking()
                .Include(x => x.Employee)
                    .ThenInclude(e => e.Organization)
                .Where(x => payPeriodIds.Contains(x.PayPeriodID.Value))
                .ToListAsync();
        }

        /// <summary>
        /// Get a list of paystub by pay period ID including employee, position and division details.
        /// Also including the entities needed by Thirteenth Month Pay calculations like Thirteenth Month Pay entity
        /// and allowance items.
        /// </summary>
        /// <param name="payPeriodId">The Id of the pay period.</param>
        /// <returns>List of paystubs.</returns>
        public async Task<ICollection<Paystub>> GetByPayPeriodWithEmployeeDivisionAndThirteenthMonthPayDetailsAsync(int payPeriodId)
        {
            return await CreateBaseQueryByPayPeriodWithEmployeeDivision(payPeriodId)
                .Include(x => x.ThirteenthMonthPay)
                .Include(x => x.AllowanceItems)
                .ToListAsync();
        }

        /// <summary>
        /// Get a list of paystub by pay period ID including employee, position and division details.
        /// </summary>
        /// <param name="payPeriodId">The Id of the pay period.</param>
        /// <returns>List of paystubs.</returns>
        public async Task<ICollection<Paystub>> GetByPayPeriodWithEmployeeDivisionAsync(int payPeriodId)
        {
            return await CreateBaseQueryByPayPeriodWithEmployeeDivision(payPeriodId)
                .ToListAsync();
        }

        public async Task<Paystub> GetWithPayPeriod(int id)
        {
            return await _context.Paystubs
                .Include(x => x.PayPeriod)
                .Where(x => x.RowID == id)
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<Paystub>> GetWithPayPeriod(int[] ids)
        {
            return await _context.Paystubs
                .Include(x => x.PayPeriod)
                .Where(x => ids.Contains(x.RowID.Value))
                .ToListAsync();
        }

        public async Task<bool> HasPaystubsAfterDateAsync(DateTime date, int employeeId)
        {
            return await _context.Paystubs
                .Where(x => x.EmployeeID == employeeId)
                .Where(x => x.PayFromDate >= date)
                .AnyAsync();
        }

        #endregion Queries

        #region private methods

        /// <summary>
        /// Resets the last transactions of the affected Leave Ledger.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="paystub">Has the leave transactions that will be used to reset the Leave Ledgers' last transactions.</param>
        private void ResetLeaveLedgerTransactions(
            ICollection<LeaveTransaction> toBeDeletedleaveTransactions,
            int userId)
        {
            // update the leaveledgers' last transaction Ids and in turn resets the balance
            var groupedLeaves = toBeDeletedleaveTransactions.GroupBy(x => x.LeaveLedger);
            var toBeDeletedleaveTransactionIds = toBeDeletedleaveTransactions
                .Select(x => x.RowID.Value)
                .Distinct()
                .ToArray();

            var leaveLedgerIds = groupedLeaves.Select(x => x.Key.RowID.Value).ToArray();
            var allLeaveTransactions = _context.LeaveTransactions
                .Where(x => leaveLedgerIds.Contains(x.LeaveLedgerID.Value))
                .Where(x => toBeDeletedleaveTransactionIds.Contains(x.RowID.Value) == false)
                .ToList();

            foreach (var leaveGroup in groupedLeaves)
            {
                var leaveLedger = leaveGroup.Key;
                var leaveTransactions = allLeaveTransactions.Where(x => x.LeaveLedgerID == leaveLedger.RowID);

                // get the last transaction
                if (leaveTransactions.Any())
                {
                    var lastTransactionDate = leaveTransactions
                        .OrderByDescending(x => x.TransactionDate)
                        .Select(x => x.TransactionDate)
                        .First();

                    var lastTransactions = leaveTransactions.Where(x => x.TransactionDate == lastTransactionDate);

                    var lastDebitTransaction = lastTransactions
                        .Where(x => x.Type.ToTrimmedLowerCase() ==
                            LeaveTransactionType.Debit.ToTrimmedLowerCase())
                        .OrderBy(x => x.Balance)
                        .FirstOrDefault();

                    var lastCreditTransaction = lastTransactions
                        .Where(x => x.Type.ToTrimmedLowerCase() ==
                            LeaveTransactionType.Credit.ToTrimmedLowerCase())
                        .OrderByDescending(x => x.Created)
                        .FirstOrDefault();

                    // If there is a Credit Transaction on the last transaction date, check if
                    // what transaction was created last: is it the last debit transaction or
                    // the last credit transaction. Since the user could have reset the leave balance
                    // after the paystub was deleted so that resetted balance should still be the balance
                    // regardless if the paystub was deleted.

                    int? lastTransactionId = null;

                    if (lastCreditTransaction == null)
                    {
                        if (lastDebitTransaction.RowID != null)
                        {
                            lastTransactionId = lastDebitTransaction.RowID;
                        }
                        else
                        {
                            // this else here means both lastCreditTransaction
                            // and lastDebitTransaction are null.
                            lastTransactionId = null;
                        }
                    }
                    else
                    {
                        if (lastDebitTransaction?.RowID == null)
                        {
                            lastTransactionId = lastCreditTransaction.RowID;
                        }
                        else
                        {
                            // lastCreditTransaction and lastDebitTransaction both have values

                            // A use case where the user transacted for that date that resulted to
                            // 10 balance then reset to 40 then transacted again that resulted to 32 balance
                            // then another transaction that resulted to 24 balance, the system should choose
                            // the 24 balance as the latest because it is the lowest balance after the credit.

                            var debitTransactionsAfterCredit = lastTransactions
                                .Where(x => x.Type.ToTrimmedLowerCase() == LeaveTransactionType.Debit.ToTrimmedLowerCase())
                                .Where(x => x.Created >= lastCreditTransaction.Created)
                                .OrderBy(x => x.Balance)
                                .FirstOrDefault();

                            if (debitTransactionsAfterCredit != null)
                            {
                                lastTransactionId = debitTransactionsAfterCredit.RowID;
                            }
                            else
                            {
                                lastTransactionId = lastCreditTransaction.RowID;
                            }
                        }
                    }

                    if (lastTransactionId != null)
                    {
                        leaveLedger.LastTransactionID = lastTransactionId;
                        leaveLedger.LastUpdBy = userId;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the deducted amount on the loans based on the passed paystub.
        /// </summary>
        /// <param name="loanTransactions">The loan transactions to be deleted with its Loan.</param>
        private void ResetLoanBalances(ICollection<LoanTransaction> loanTransactions, int userId)
        {
            // update the employeeloanschedules' balance
            var groupedLoans = loanTransactions.GroupBy(x => x.Loan);

            foreach (var loanGroup in groupedLoans)
            {
                var loan = loanGroup.Key;
                var loanTransaction = loanGroup.Select(x => x);

                loan.RetractLoanTransactions(loanTransaction);
                loan.LastUpdBy = userId;
            }
        }

        private IQueryable<Paystub> CreateBaseQueryByPayPeriodWithEmployeeDivision(int payPeriodId)
        {
            return _context.Paystubs
                .Include(x => x.Employee.Position.Division)
                .Where(x => x.PayPeriodID == payPeriodId);
        }

        private IQueryable<Paystub> CreateBaseQueryWithFullPaystub()
        {
            return _context.Paystubs
                .AsNoTracking()
                .Include(p => p.Employee.Position.Division)
                .Include(p => p.Adjustments)
                    .ThenInclude(a => a.Product)
                .Include(p => p.ActualAdjustments)
                    .ThenInclude(a => a.Product)
                .Include(p => p.LoanTransactions)
                    .ThenInclude(a => a.Loan)
                        .ThenInclude(a => a.LoanType)
                // Allowance not included yet since it is not needed currently
                .Include(p => p.AllowanceItems)
                .Include(p => p.ThirteenthMonthPay)
                .Include(p => p.Actual)
                .Include(p => p.LoanPaymentFromThirteenthMonthPays);
        }

        private IQueryable<Paystub> AddGetByEmployeeCompositeKeyQuery(EmployeeCompositeKey key, IQueryable<Paystub> query)
        {
            query = query
                .Where(x => x.EmployeeID == key.EmployeeId)
                .Where(x => x.PayPeriodID == key.PayPeriodId);

            return query;
        }

        #endregion private methods
    }
}
