using AccuPay.Data.Entities;
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
        public class CompositeKey
        {
            public int EmployeeId { get; set; }
            public int PayPeriodId { get; set; }

            public CompositeKey(int employeeId, int payPeriodId)
            {
                EmployeeId = employeeId;

                PayPeriodId = payPeriodId;
            }
        }

        public async Task DeleteAsync(CompositeKey key, int userId)
        {
            int? paystubId = null;

            using (var context = new PayrollContext())
            {
                paystubId = context.Paystubs.
                     Where(x => x.EmployeeID == key.EmployeeId).
                     Where(x => x.PayPeriodID == key.PayPeriodId).
                     Select(x => x.RowID).
                     FirstOrDefault();
            }

            if (paystubId == null)
            {
                // maybe throw an error?
                return;
            }

            await DeleteAsync(id: paystubId.Value,
                              userId: userId);
        }

        public async Task DeleteByPeriodAsync(int payPeriodId, int userId)
        {
            using (var context = new PayrollContext())
            {
                var payStubIds = await context.Paystubs.
                                                Where(x => x.PayPeriodID == payPeriodId).
                                                Select(x => x.RowID.Value).
                                                ToListAsync();

                foreach (int id in payStubIds)
                {
                    await DeleteAsyncWithContext(id: id,
                                                userId: userId,
                                                context: context);
                }

                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id, int userId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                await DeleteAsyncWithContext(id: id,
                                            userId: userId,
                                            context: context);

                await context.SaveChangesAsync();
            }
        }

        private static async Task DeleteAsyncWithContext(int id, int userId, PayrollContext context)
        {
            var paystub = await context.Paystubs.
                                            Include(x => x.LoanTransactions).
                                                ThenInclude(x => x.LoanSchedule).
                                            Include(x => x.LeaveTransactions).
                                                ThenInclude(x => x.LeaveLedger).

                                            Include(x => x.Adjustments).
                                            Include(x => x.ActualAdjustments).
                                            Include(x => x.PaystubItems).
                                            Include(x => x.PaystubEmails).
                                            Include(x => x.PaystubEmailHistories).

                                            Include(x => x.ThirteenthMonthPay).
                                            Include(x => x.Actual).
                                            Where(x => x.RowID == id).
                                            FirstOrDefaultAsync();

            // Some of this are already deleted cascadingly if we delete the paystub but the
            // aggregate rot should handle the delete of of all the entities in its aggregate.
            // AND some database from other clients have cascade constraint, some have not.

            // Deleting of paystub bonus is not yet supported. Bonus will be disabled since
            // no AccuPay clients are using it.

            ResetLoanBalances(paystub.LoanTransactions, userId);
            context.LoanTransactions.RemoveRange(paystub.LoanTransactions);

            // RemoveRange does not delete the entities immediately so the order of
            // resetting the parent entity and deleting that child are not important
            ResetLeaveLedgerTransactions(context, paystub.LeaveTransactions, userId);
            context.LeaveTransactions.RemoveRange(paystub.LeaveTransactions);

            context.AllowanceItems.RemoveRange(paystub.AllowanceItems);
            context.Adjustments.RemoveRange(paystub.Adjustments);
            context.ActualAdjustments.RemoveRange(paystub.ActualAdjustments);
            context.PaystubEmails.RemoveRange(paystub.PaystubEmails);
            context.PaystubEmailHistories.RemoveRange(paystub.PaystubEmailHistories);
            context.PaystubItems.RemoveRange(paystub.PaystubItems);

            context.ThirteenthMonthPays.Remove(paystub.ThirteenthMonthPay);
            context.PaystubActuals.Remove(paystub.Actual);

            context.Paystubs.Remove(paystub);
        }

        #region Queries

        public async Task<Paystub> GetByCompositeKeyFullPaystubAsync(CompositeKey key)
        {
            using (var context = new PayrollContext())
            {
                return await CreateBaseQueryWithFullPaystub(context).
                                Where(x => x.EmployeeID == key.EmployeeId).
                                Where(x => x.PayPeriodID == key.PayPeriodId).
                                FirstOrDefaultAsync();
            }
        }

        public async Task<IEnumerable<Paystub>> GetByPayPeriodFullPaystubAsync(int payPeriodId)
        {
            using (var context = new PayrollContext())
            {
                return await CreateBaseQueryWithFullPaystub(context).
                                Where(x => x.PayPeriodID == payPeriodId).
                                ToListAsync();
            }
        }

        public async Task<IEnumerable<Paystub>> GetPreviousCutOffPaystubsAsync(
                                            DateTime currentCuttOffStart,
                                            int organizationId)
        {
            var previousCutoffEnd = currentCuttOffStart.AddDays(-1);

            using (var context = new PayrollContext())
            {
                return await context.Paystubs.
                                Where(x => x.PayToDate == previousCutoffEnd).
                                Where(x => x.OrganizationID == organizationId).
                                ToListAsync();
            }
        }

        public async Task<IEnumerable<Paystub>> GetByPayPeriodWithEmployeeAsync(int payPeriodId)
        {
            using (var context = new PayrollContext())
            {
                return await context.Paystubs.
                                Include(x => x.Employee).
                                Where(x => x.PayPeriodID == payPeriodId).
                                ToListAsync();
            }
        }

        #region Child data

        public async Task<IEnumerable<AllowanceItem>> GetAllowanceItems(int paystubId)
        {
            using (var context = new PayrollContext())
            {
                var paystub = await context.Paystubs.
                                Include(x => x.AllowanceItems).
                                    ThenInclude(x => x.Allowance).
                                        ThenInclude(x => x.Product).
                                Where(x => x.RowID == paystubId).
                                FirstOrDefaultAsync();

                return paystub.AllowanceItems;
            }
        }

        public async Task<IEnumerable<LoanTransaction>> GetLoanTransanctions(int paystubId)
        {
            using (var context = new PayrollContext())
            {
                var paystub = await context.Paystubs.
                                Include(x => x.LoanTransactions).
                                    ThenInclude(x => x.LoanSchedule).
                                        ThenInclude(x => x.LoanType).
                                Where(x => x.RowID == paystubId).
                                FirstOrDefaultAsync();

                return paystub.LoanTransactions;
            }
        }

        #endregion Child data

        #endregion Queries

        /// <summary>
        /// Resets the last transactions of the affected Leave Ledger.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="paystub">Has the leave transactions that will be used to reset the Leave Ledgers' last transactions.</param>
        private static void ResetLeaveLedgerTransactions(
                                        PayrollContext context,
                                        ICollection<LeaveTransaction> toBeDeletedleaveTransactions,
                                        int userId)
        {
            // update the leaveledgers' last transaction Ids and in turn resets the balance
            var groupedLeaves = toBeDeletedleaveTransactions.GroupBy(x => x.LeaveLedger);
            var toBeDeletedleaveTransactionIds = toBeDeletedleaveTransactions.Select(x => x.RowID.Value).ToArray();

            var leaveLedgerIds = groupedLeaves.Select(x => x.Key.RowID.Value).ToArray();
            var allLeaveTransactions = context.LeaveTransactions.
                                            Where(x => leaveLedgerIds.Contains(x.LeaveLedgerID.Value)).
                                            Where(x => toBeDeletedleaveTransactionIds.Contains(x.RowID.Value) == false).
                                            ToList();

            foreach (var leaveGroup in groupedLeaves)
            {
                var leaveLedger = leaveGroup.Key;
                var leaveTransactions = allLeaveTransactions.
                                            Where(x => x.LeaveLedgerID == leaveLedger.RowID);

                // get the last transaction
                if (leaveTransactions.Any())
                {
                    var lastTransactionDate = leaveTransactions.
                                                OrderByDescending(x => x.TransactionDate).
                                                Select(x => x.TransactionDate).
                                                First();

                    var lastTransactions = leaveTransactions.
                                                Where(x => x.TransactionDate == lastTransactionDate);

                    var lastDebitTransaction = lastTransactions.
                                                Where(x => x.Type.ToTrimmedLowerCase() ==
                                                    LeaveTransactionType.Debit.ToTrimmedLowerCase()).
                                                OrderByDescending(x => x.Balance).
                                                FirstOrDefault();

                    var lastCreditTransaction = lastTransactions.
                                                Where(x => x.Type.ToTrimmedLowerCase() ==
                                                    LeaveTransactionType.Credit.ToTrimmedLowerCase()).
                                                OrderByDescending(x => x.Created).
                                                FirstOrDefault();

                    // If there is a Credit Transaction on the last transaction date, check if
                    // what transaction was created last: is it the last debit transaction or
                    // the last credit transaction. Since the user could have reset the leave balance
                    // after the paystub so that reset balance should still be the balance regardless
                    // if the paystub is deleted.

                    int? lastTransactionId = null;

                    if (lastCreditTransaction == null)
                    {
                        if (lastDebitTransaction.RowID != null)
                        {
                            lastTransactionId = lastDebitTransaction.RowID;
                        }
                    }
                    else
                    {
                        if (lastDebitTransaction.RowID == null)
                        {
                            lastTransactionId = lastCreditTransaction.RowID;
                        }
                        else
                        {
                            var lastTransaction = new List<LeaveTransaction>()
                                                        { lastDebitTransaction, lastCreditTransaction }.
                                                    OrderByDescending(x => x.Created).
                                                    First();

                            lastTransactionId = lastTransaction.RowID;
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

                loan.TotalBalanceLeft += loanGroup.Sum(x => x.Amount);
                loan.LastUpdBy = userId;
            }
        }

        private IQueryable<Paystub> CreateBaseQueryWithFullPaystub(PayrollContext context)
        {
            return context.Paystubs.Include(p => p.Adjustments).
                                        ThenInclude(a => a.Product).
                                    Include(p => p.ActualAdjustments).
                                        ThenInclude(a => a.Product).

                                    Include(p => p.LoanTransactions).
                                        ThenInclude(a => a.LoanSchedule).
                                            ThenInclude(a => a.LoanType).

                                    // Allowance not included yet since it is not needed currently
                                    Include(p => p.AllowanceItems).
                                    Include(p => p.ThirteenthMonthPay).
                                    Include(p => p.Actual);
        }
    }
}