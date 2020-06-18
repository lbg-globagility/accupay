using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class PaystubRepository
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

        #region CRUD

        public async Task DeleteAsync(EmployeeCompositeKey key, int userId)
        {
            int? paystubId = null;

            paystubId = _context.Paystubs
                .Where(x => x.EmployeeID == key.EmployeeId)
                .Where(x => x.PayPeriodID == key.PayPeriodId)
                .Select(x => x.RowID)
                .FirstOrDefault();

            if (paystubId == null)
            {
                // maybe throw an error?
                return;
            }

            await DeleteAsync(id: paystubId.Value,
                              userId: userId);
        }

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

        public async Task UpdateManyThirteenthMonthPaysAsync(IEnumerable<ThirteenthMonthPay> thirteenthMonthPays)
        {
            foreach (var thirteenthMonthPay in thirteenthMonthPays)
            {
                _context.Entry(thirteenthMonthPay).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
        }

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

        #endregion CRUD

        #region Queries

        #region Child data

        public async Task<IEnumerable<AllowanceItem>> GetAllowanceItemsAsync(int paystubId)
        {
            var paystub = await _context.Paystubs
                .Include(x => x.AllowanceItems)
                    .ThenInclude(x => x.Allowance)
                        .ThenInclude(x => x.Product)
                .Where(x => x.RowID == paystubId)
                .FirstOrDefaultAsync();

            return paystub.AllowanceItems;
        }

        public async Task<IEnumerable<LoanTransaction>> GetLoanTransanctionsAsync(int paystubId)
        {
            var paystub = await _context.Paystubs
                .Include(x => x.LoanTransactions)
                    .ThenInclude(x => x.LoanSchedule)
                        .ThenInclude(x => x.LoanType)
                .Where(x => x.RowID == paystubId)
                .FirstOrDefaultAsync();

            return paystub.LoanTransactions;
        }

        #endregion Child data

        public Paystub GetByCompositeKeyWithActual(EmployeeCompositeKey key)
        {
            return _context.Paystubs
                .AsNoTracking()
                .Include(x => x.ThirteenthMonthPay)
                .Include(x => x.Actual)
                .Where(x => x.EmployeeID == key.EmployeeId)
                .Where(x => x.PayPeriodID == key.PayPeriodId)
                .FirstOrDefault();
        }

        public async Task<Paystub> GetByCompositeKeyFullPaystubAsync(EmployeeCompositeKey key)
        {
            return await CreateBaseQueryWithFullPaystub()
                .Where(x => x.EmployeeID == key.EmployeeId)
                .Where(x => x.PayPeriodID == key.PayPeriodId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Paystub>> GetByPayPeriodFullPaystubAsync(int payPeriodId)
        {
            return await CreateBaseQueryWithFullPaystub()
                .Where(x => x.PayPeriodID == payPeriodId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Paystub>> GetPreviousCutOffPaystubsAsync(
            DateTime currentCuttOffStart,
            int organizationId)
        {
            var previousCutoffEnd = currentCuttOffStart.AddDays(-1);

            return await _context.Paystubs
                .Where(x => x.PayToDate == previousCutoffEnd)
                .Where(x => x.OrganizationID == organizationId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Paystub>> GetAllWithEmployeeAsync(DateCompositeKey key)
        {
            return await _context.Paystubs
                .Where(x => x.PayFromDate == key.PayFromDate)
                .Where(x => x.PayToDate == key.PayToDate)
                .Where(x => x.OrganizationID == key.OrganizationId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Paystub>> GetByPayPeriodWithEmployeeAsync(int payPeriodId)
        {
            return await _context.Paystubs
                .Include(x => x.Employee)
                .Where(x => x.PayPeriodID == payPeriodId)
                .ToListAsync();
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

        private IQueryable<Paystub> CreateBaseQueryByPayPeriodWithEmployeeDivision(int payPeriodId)
        {
            return _context.Paystubs
                .Include(x => x.Employee.Position.Division)
                .Where(x => x.PayPeriodID == payPeriodId);
        }

        public async Task<PaginatedListResult<Paystub>> GetPaginatedListAsync(
            PageOptions options,
            int payPeriodId,
            string searchTerm = "")
        {
            var query = CreateBaseQueryByPayPeriodWithEmployeeDivision(payPeriodId)
                .OrderBy(x => x.Employee.LastName)
                .ThenBy(x => x.Employee.FirstName)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = $"%{searchTerm}%";

                query = query.Where(x =>
                    EF.Functions.Like(x.Employee.EmployeeNo, searchTerm) ||
                    EF.Functions.Like(x.Employee.FirstName, searchTerm) ||
                    EF.Functions.Like(x.Employee.LastName, searchTerm));
            }

            var paystubs = await query.Page(options).ToListAsync();
            var count = await query.CountAsync();

            return new PaginatedListResult<Paystub>(paystubs, count);
        }

        #endregion Queries

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
            var toBeDeletedleaveTransactionIds = toBeDeletedleaveTransactions.
                                                    Select(x => x.RowID.Value).
                                                    ToArray();

            var leaveLedgerIds = groupedLeaves.Select(x => x.Key.RowID.Value).ToArray();
            var allLeaveTransactions = _context.LeaveTransactions
                .Where(x => leaveLedgerIds.Contains(x.LeaveLedgerID.Value))
                .Where(x => toBeDeletedleaveTransactionIds.Contains(x.RowID.Value) == false)
                .ToList();

            foreach (var leaveGroup in groupedLeaves)
            {
                var leaveLedger = leaveGroup.Key;
                var leaveTransactions = allLeaveTransactions.
                                            Where(x => x.LeaveLedgerID == leaveLedger.RowID);

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
                                                        .Where(x => x.Type.ToTrimmedLowerCase() ==
                                                            LeaveTransactionType.Debit.ToTrimmedLowerCase())
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
        private static void ResetLoanBalances(ICollection<LoanTransaction> loanTransactions, int userId)
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

        private IQueryable<Paystub> CreateBaseQueryWithFullPaystub()
        {
            return _context.Paystubs
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
    }
}