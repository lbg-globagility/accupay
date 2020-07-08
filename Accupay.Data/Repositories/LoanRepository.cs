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
    public class LoanRepository : SavableRepository<LoanSchedule>
    {
        public LoanRepository(PayrollContext context) : base(context)
        {
        }

        /// <summary>
        /// 'delete all loans that are not HDMF or SSS. only HDMF or SSS loans are supported in benchmark
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="pagibigLoanId"></param>
        /// <param name="ssLoanId"></param>
        /// <returns></returns>
        internal async Task DeleteAllLoansExceptGovernmentLoansAsync(
            int employeeId,
            int pagibigLoanId,
            int ssLoanId)
        {
            _context.LoanSchedules
                .RemoveRange(_context.LoanSchedules
                    .Where(x => x.EmployeeID == employeeId)
                    .Where(x => x.LoanTypeID != pagibigLoanId)
                    .Where(x => x.LoanTypeID != ssLoanId));

            await _context.SaveChangesAsync();
        }

        protected override void DetachNavigationProperties(LoanSchedule loan)
        {
            if (loan.Employee != null)
            {
                _context.Entry(loan.Employee).State = EntityState.Detached;
            }

            if (loan.LoanType != null)
            {
                _context.Entry(loan.LoanType).State = EntityState.Detached;

                if (loan.LoanType.CategoryEntity != null)
                {
                    _context.Entry(loan.LoanType.CategoryEntity).State = EntityState.Detached;
                }
            }
        }

        #region Queries

        #region Single entity

        public async Task<LoanSchedule> GetByIdWithEmployeeAndProductAsync(int id)
        {
            return await _context.LoanSchedules
                .Include(x => x.Employee)
                .Include(x => x.LoanType)
                .FirstOrDefaultAsync(l => l.RowID == id);
        }

        #endregion Single entity

        #region List of entities

        public async Task<ICollection<LoanSchedule>> GetByEmployeeAsync(int employeeId)
        {
            return await _context.LoanSchedules
                .Where(l => l.EmployeeID == employeeId)
                .ToListAsync();
        }

        public async Task<PaginatedListResult<LoanSchedule>> GetPaginatedListAsync(PageOptions options, int organizationId, string searchTerm = "")
        {
            var query = _context.LoanSchedules
                .Include(x => x.Employee)
                .Include(x => x.LoanType)
                .Where(x => x.OrganizationID == organizationId)
                .OrderByDescending(x => x.DedEffectiveDateFrom)
                .ThenBy(x => x.LoanType.PartNo)
                .ThenBy(x => x.Employee.LastName)
                .ThenBy(x => x.Employee.FirstName)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = $"%{searchTerm}%";

                query = query.Where(x =>
                    EF.Functions.Like(x.LoanType.PartNo, searchTerm) ||
                    EF.Functions.Like(x.Employee.EmployeeNo, searchTerm) ||
                    EF.Functions.Like(x.Employee.FirstName, searchTerm) ||
                    EF.Functions.Like(x.Employee.LastName, searchTerm));
            }

            var loanSchedules = await query.Page(options).ToListAsync();
            var count = await query.CountAsync();

            return new PaginatedListResult<LoanSchedule>(loanSchedules, count);
        }

        public async Task<PaginatedListResult<LoanTransaction>> GetLoanTransactionsAsync(PageOptions options, int loanId)
        {
            var query = _context.LoanTransactions
                .Include(x => x.PayPeriod)
                .Include(x => x.LoanSchedule.Employee)
                .Where(x => x.LoanScheduleID == loanId)
                .OrderByDescending(x => x.PayPeriod.PayToDate)
                .AsQueryable();

            var transactions = await query.Page(options).ToListAsync();
            var count = await query.CountAsync();

            return new PaginatedListResult<LoanTransaction>(transactions, count);
        }

        public async Task<ICollection<LoanSchedule>> GetActiveLoansByLoanNameAsync(string loanName, int employeeId)
        {
            return await _context.LoanSchedules
                .Include(l => l.LoanType)
                .Include(l => l.LoanType.CategoryEntity)
                .Where(l => l.LoanType.CategoryEntity.CategoryName.Trim().ToUpper() == ProductConstant.LOAN_TYPE_CATEGORY.Trim().ToUpper())
                .Where(l => l.LoanType.PartNo.Trim().ToLower() == loanName.ToTrimmedLowerCase())
                .Where(l => l.Status.Trim().ToLower() == LoanSchedule.STATUS_IN_PROGRESS.ToTrimmedLowerCase())
                .Where(l => l.EmployeeID == employeeId)
                .ToListAsync();
        }

        public async Task<ICollection<LoanTransaction>> GetLoanTransactionsWithPayPeriodAsync(int loanScheduleId)
        {
            return await _context.LoanTransactions
                .Include(l => l.PayPeriod)
                .Where(l => l.LoanScheduleID == loanScheduleId)
                .ToListAsync();
        }

        /// <summary>
        /// This function is for the specific needs of PayrollGeneration. Analyze the code below before using.
        /// This returns all loans that are IN PROGRESS and loans that may be ON HOLD, CANCELLED or COMPLETE as long
        /// as this has been used by the current payroll.
        /// </summary>
        /// <param name="organizationId">Current organization ID.</param>
        /// <param name="payPeriod">Current PayPeriod object.</param>
        /// <param name="paystubs">Used to check if the loans were used in the current payroll even if it is not IN PROGRESS.</param>
        /// <returns></returns>
        internal async Task<ICollection<LoanSchedule>> GetCurrentPayrollLoansAsync(
            int organizationId,
            PayPeriod payPeriod,
            IReadOnlyCollection<Paystub> paystubs)
        {
            string[] acceptedLoans = new string[] { };
            if (payPeriod.IsFirstHalf)
            {
                acceptedLoans = new[] { ContributionSchedule.PER_PAY_PERIOD, ContributionSchedule.FIRST_HALF };
            }
            else if (payPeriod.IsEndOfTheMonth)
            {
                acceptedLoans = new[] { ContributionSchedule.PER_PAY_PERIOD, ContributionSchedule.END_OF_THE_MONTH };
            }

            // Get all even if it is Cancelled, On Hold or Completed.
            // Even if it is not In Progress, if that loan is used by the current payroll,
            // example, initially it was in progress but after the current payroll it was completed.
            // That is still needed in regenerating the payroll as that may be Cancelled or put on Hold
            // before the regeneration if the user chose to exclude it this pay period. On payroll regeneration,
            // this loan's balance should be reset and this will not apply to the current payroll.
            var loans = await _context.LoanSchedules
                .Include(l => l.LoanPaymentFromBonuses)
                .Where(l => l.OrganizationID == organizationId)
                .Where(l => l.DedEffectiveDateFrom <= payPeriod.PayToDate)
                .Where(l => acceptedLoans.Contains(l.DeductionSchedule.Trim().ToUpper()))
                .Where(l => l.BonusID == null)
                .ToListAsync();

            var currentLoans = new List<LoanSchedule>();

            // get IN PROGRESS loans
            var inProgressLoans = loans.Where(x => x.Status == LoanSchedule.STATUS_IN_PROGRESS);
            currentLoans.AddRange(inProgressLoans);

            // get not IN PROGRESS loans but has loantransactions for this payperiod
            // (probably was completed this pay period or the loan was edited as
            // ON HOLD or CANCELLED to exclude that loan this pay period)
            var notInProgressLoans = loans.Where(x => x.Status != LoanSchedule.STATUS_IN_PROGRESS);
            var currentLoanTransactions = paystubs.SelectMany(x => x.LoanTransactions);

            var notInProgressLoansWithTransaction = notInProgressLoans
                .Where(x => currentLoanTransactions
                    .Any(t => t.LoanScheduleID == x.RowID));

            currentLoans.AddRange(notInProgressLoansWithTransaction);

            return currentLoans;
        }

        #endregion List of entities

        #region Others

        public List<string> GetStatusList()
        {
            return new List<string>()
            {
                LoanSchedule.STATUS_IN_PROGRESS,
                LoanSchedule.STATUS_ON_HOLD,
                LoanSchedule.STATUS_CANCELLED,
                LoanSchedule.STATUS_COMPLETE
            };
        }

        #endregion Others

        #endregion Queries
    }
}