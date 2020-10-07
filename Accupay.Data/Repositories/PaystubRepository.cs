using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.ValueObjects;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class PaystubRepository : BaseRepository
    {
        private readonly PayrollContext _context;

        public PaystubRepository(PayrollContext context)
        {
            _context = context;
        }

        public class EmployeeCompositeKey
        {
            public int EmployeeId { get; set; }
            public int PayPeriodId { get; set; }

            public EmployeeCompositeKey(int employeeId, int payPeriodId)
            {
                EmployeeId = employeeId;
                PayPeriodId = payPeriodId;
            }
        }

        public class DateCompositeKey
        {
            public int OrganizationId { get; set; }
            public DateTime PayFromDate { get; set; }
            public DateTime PayToDate { get; set; }

            public DateCompositeKey(int organizationId, DateTime payFromDate, DateTime payToDate)
            {
                OrganizationId = organizationId;
                PayFromDate = payFromDate;
                PayToDate = payToDate;
            }
        }

        #region Save

        public async Task DeleteAsync(int id, int userId)
        {
            await DeleteAsyncWithContext(id: id, userId: userId);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByPeriodAsync(int payPeriodId, int userId)
        {
            var payStubIds = await _context.Paystubs
                .Where(x => x.PayPeriodID == payPeriodId)
                .Select(x => x.RowID.Value)
                .ToListAsync();

            foreach (int id in payStubIds)
            {
                await DeleteAsyncWithContext(id: id, userId: userId);
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateManyThirteenthMonthPaysAsync(ICollection<ThirteenthMonthPay> thirteenthMonthPays)
        {
            foreach (var thirteenthMonthPay in thirteenthMonthPays)
            {
                _context.Entry(thirteenthMonthPay).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
        }

        #region UpdateAdjustments

        public async Task UpdateAdjustmentsAsync<T>(int paystubId, ICollection<T> allAdjustments) where T : IAdjustment
        {
            Paystub paystub = await GetPaystubWithAdjustments(paystubId);

            decimal originalDeclaredAdjustments = paystub.Adjustments.Sum(x => x.Amount);
            decimal originalActualAdjustments = paystub.ActualAdjustments.Sum(x => x.Amount);
            decimal originalTotalAdjustments = originalDeclaredAdjustments + originalActualAdjustments;

            List<T> originalAdjustments = GetOriginalAdjustments<T>(paystub);

            List<T> modifiedAdjustments = GetModifiedAdjustments(allAdjustments);

            AddContextState(paystub, originalAdjustments, modifiedAdjustments);

            // Save changes
            decimal updatedTotalAdjustments = paystub.Adjustments.Sum(x => x.Amount) + paystub.ActualAdjustments.Sum(x => x.Amount);

            if (typeof(Adjustment).IsAssignableFrom(typeof(T)))
            {
                paystub.TotalAdjustments = paystub.Adjustments.Sum(x => x.Amount);
                paystub.NetPay = (paystub.NetPay - originalDeclaredAdjustments) + paystub.TotalAdjustments;
            }

            paystub.Actual.TotalAdjustments = updatedTotalAdjustments;
            paystub.Actual.NetPay = (paystub.Actual.NetPay - originalTotalAdjustments) + paystub.Actual.TotalAdjustments;

            _context.Entry(paystub).State = EntityState.Modified;
            _context.Entry(paystub.Actual).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        private static List<T> GetOriginalAdjustments<T>(Paystub paystub) where T : IAdjustment
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

        private void AddContextState<T>(Paystub paystub, List<T> originalAdjustments, List<T> modifiedAdjustments) where T : IAdjustment
        {
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
                    _context.Entry(adjustment).State = EntityState.Deleted;
                }
                else
                {
                    adjustment.Amount = updatedAdjustment.Amount;
                    adjustment.ProductID = updatedAdjustment.ProductID;
                    adjustment.Comment = updatedAdjustment.Comment;

                    _context.Entry(adjustment).State = EntityState.Modified;
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

                _context.Entry(newAdjustment).State = EntityState.Added;
            }
        }

        #endregion UpdateAdjustments

        private async Task DeleteAsyncWithContext(int id, int userId)
        {
            var paystub = await _context.Paystubs
                .Include(x => x.LoanTransactions)
                    .ThenInclude(x => x.LoanSchedule)
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
                    .ThenInclude(x => x.LoanSchedule)
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

        public async Task<ICollection<Paystub>> GetPreviousCutOffPaystubsAsync(
            DateTime currentCuttOffStart,
            int organizationId)
        {
            var previousCutoffEnd = currentCuttOffStart.AddDays(-1);

            return await _context.Paystubs
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

        public async Task<ICollection<Paystub>> GetByPayPeriodWithEmployeeAsync(int payPeriodId)
        {
            return await _context.Paystubs
                .Include(x => x.Employee)
                .Where(x => x.PayPeriodID == payPeriodId)
                .ToListAsync();
        }

        public async Task<bool> HasPaystubsAfterDateAsync(DateTime date, int employeeId)
        {
            return await _context.Paystubs
                .Where(x => x.EmployeeID == employeeId)
                .Where(x => x.PayFromDate >= date)
                .AnyAsync();
        }

        /// <summary>
        /// Get a list of paystub by pay period ID including employee, position and division details.
        /// Also including the entities needed by Thirteenth Month Pay calculations like Thirteenth Month Pay entity
        /// and allowance items.
        /// </summary>
        /// <param name="payPeriodId"></param>
        /// <returns></returns>
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
        /// <param name="payPeriodId"></param>
        /// <returns></returns>
        public async Task<ICollection<Paystub>> GetByPayPeriodWithEmployeeDivisionAsync(int payPeriodId)
        {
            return await CreateBaseQueryByPayPeriodWithEmployeeDivision(payPeriodId)
                .ToListAsync();
        }

        public async Task<ICollection<Paystub>> GetAllAsync(int payPeriodId)
        {
            var query = CreateBaseQueryByPayPeriodWithEmployeeDivision(payPeriodId)
                .OrderBy(x => x.Employee.LastName)
                .ThenBy(x => x.Employee.FirstName)
                .AsQueryable();

            return await query.ToListAsync();
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
        /// <param name="loanTransactions">The loan transactions to be deleted with its LoanSchedule.</param>
        private void ResetLoanBalances(ICollection<LoanTransaction> loanTransactions, int userId)
        {
            // update the employeeloanschedules' balance
            var groupedLoans = loanTransactions.GroupBy(x => x.LoanSchedule);

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
                .Include(p => p.Employee.Position.Division)
                .Include(p => p.Adjustments)
                    .ThenInclude(a => a.Product)
                .Include(p => p.ActualAdjustments)
                    .ThenInclude(a => a.Product)
                .Include(p => p.LoanTransactions)
                    .ThenInclude(a => a.LoanSchedule)
                        .ThenInclude(a => a.LoanType)
                // Allowance not included yet since it is not needed currently
                .Include(p => p.AllowanceItems)
                .Include(p => p.ThirteenthMonthPay)
                .Include(p => p.Actual);
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