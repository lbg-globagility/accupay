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

        public async Task DeleteAsync(CompositeKey key)
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

            await DeleteAsync(paystubId.Value);
        }

        public async Task DeleteAsync(int id)
        {
            using (PayrollContext context = new PayrollContext())
            {
                var paystub = await context.Paystubs.
                                Include(x => x.LoanTransactions).
                                    ThenInclude(x => x.LoanSchedule).
                                Include(x => x.LeaveTransactions).
                                    ThenInclude(x => x.LeaveLedger).
                                Include(x => x.PaystubItems).
                                Include(x => x.Adjustments).
                                Include(x => x.ActualAdjustments).
                                Include(x => x.ThirteenthMonthPay).
                                Include(x => x.Actual).
                                Where(x => x.RowID == id).
                                FirstOrDefaultAsync();

                // Reset the loan balance before we remove it since we need the to be deleted
                // paystub loan transactions on recomputing the loan balance before we delete it.
                ResetLoanBalances(context, paystub);
                context.LoanTransactions.RemoveRange(paystub.LoanTransactions);

                context.LeaveTransactions.RemoveRange(paystub.LeaveTransactions);
                // Reset the leave ledger last transactions after we remove it since we need the
                // latest transaction of their leaves not including the deleted paystubs leave transactions.
                ResetLeaveLedgerTransactions(context, paystub);

                context.AllowanceItems.RemoveRange(paystub.AllowanceItems);
                context.Adjustments.RemoveRange(paystub.Adjustments);
                context.ActualAdjustments.RemoveRange(paystub.ActualAdjustments);
                context.PaystubItems.RemoveRange(paystub.PaystubItems);

                context.ThirteenthMonthPays.Remove(paystub.ThirteenthMonthPay);
                context.PaystubActuals.Remove(paystub.Actual);

                context.Paystubs.Remove(paystub);
                await context.SaveChangesAsync();
            }
        }

        public async Task GetFullPaystub(CompositeKey key)
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

            await DeleteAsync(paystubId.Value);
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
        private static void ResetLeaveLedgerTransactions(PayrollContext context, Paystub paystub)
        {
            // update the leaveledgers' last transaction Ids and in turn resets the balance
            var groupedLeaves = paystub.LeaveTransactions.GroupBy(x => x.LeaveLedger);
            var leaveLedgerIds = groupedLeaves.Select(x => x.Key.RowID.Value).ToArray();
            var allLeaveTransactions = context.LeaveTransactions.
                                            Where(x => leaveLedgerIds.Contains(x.LeaveLedgerID.Value)).
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
                                                First();

                    var lastCreditTransaction = lastTransactions.
                                                Where(x => x.Type.ToTrimmedLowerCase() ==
                                                    LeaveTransactionType.Debit.ToTrimmedLowerCase()).
                                                OrderByDescending(x => x.Created).
                                                First();

                    // If there is a Credit Transaction on the last transaction date, check if
                    // what transaction was created last: is it the last debit transaction or
                    // the last credit transaction. Since the user could have reset the leave balance
                    // after the paystub so that reset balance should still be the balance regardless
                    // if the paystub is deleted.
                    if (lastCreditTransaction == null)
                    {
                        if (lastDebitTransaction.RowID != null)
                        {
                            leaveLedger.LastTransactionID = lastDebitTransaction.RowID;
                        }
                    }
                    else
                    {
                        if (lastDebitTransaction.RowID == null)
                        {
                            leaveLedger.LastTransactionID = lastCreditTransaction.RowID;
                        }
                        else
                        {
                            var lastTransaction = new List<LeaveTransaction>()
                                                        { lastDebitTransaction, lastCreditTransaction }.
                                                    OrderByDescending(x => x.Created).
                                                    First();

                            leaveLedger.LastTransactionID = lastTransaction.RowID;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns the deducted amount on the loans based on the passed paystub.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="paystub">Has the loan transactions that will be used to reset the loan balance.</param>
        private static void ResetLoanBalances(PayrollContext context, Paystub paystub)
        {
            // update the employeeloanschedules' balance
            var groupedLoans = paystub.LoanTransactions.GroupBy(x => x.LoanSchedule);

            foreach (var loanGroup in groupedLoans)
            {
                // We could have just find the loan schedule from paystub.LoanTransactions
                // but this is more accurate. It is also not expensive since we use Find() which
                // just looks up the entity that is loaded up in the context if the entity is already
                // loaded which it is since we queried it from the paystub query.
                var loan = loanGroup.Key;

                loan.TotalBalanceLeft += loanGroup.Sum(x => x.Amount);
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