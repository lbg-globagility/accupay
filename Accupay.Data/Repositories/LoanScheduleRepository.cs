using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Helpers;
using AccuPay.Data.Services;
using AccuPay.Utilities;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class LoanScheduleRepository
    {
        private readonly PayrollContext _context;
        private readonly SystemOwnerService _systemOwnerService;

        public LoanScheduleRepository(PayrollContext context, SystemOwnerService systemOwnerService)
        {
            _context = context;
            _systemOwnerService = systemOwnerService;
        }

        #region CRUD

        public async Task DeleteAsync(int loanScheduleId)
        {
            var loanSchedule = await GetByIdAsync(loanScheduleId);

            _context.Remove(loanSchedule);

            await _context.SaveChangesAsync();
        }

        public async Task SaveManyAsync(List<LoanSchedule> loanSchedules)
        {
            foreach (var loanSchedule in loanSchedules)
            {
                await SaveWithContextAsync(loanSchedule);

                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveAsync(LoanSchedule loanSchedule)
        {
            await SaveWithContextAsync(loanSchedule, deferSave: false);
        }

        private async Task SaveWithContextAsync(LoanSchedule loanSchedule, bool deferSave = true)
        {
            // if completed yung loan, hindi pwede ma i-insert or update
            if (loanSchedule.Status == LoanSchedule.STATUS_COMPLETE)
                throw new BusinessLogicException("Loan schedule is already completed!");

            if (string.IsNullOrWhiteSpace(loanSchedule.LoanName))
            {
                var loanName = await _context.Products
                                        .Where(l => l.RowID == loanSchedule.LoanTypeID)
                                        .Select(x => x.PartNo)
                                        .FirstOrDefaultAsync();

                loanSchedule.LoanName = loanName;
            }

            await ValidationForBenchmark(loanSchedule);

            // sanitize columns
            loanSchedule.TotalLoanAmount = AccuMath.CommercialRound(loanSchedule.TotalLoanAmount);
            loanSchedule.DeductionAmount = AccuMath.CommercialRound(loanSchedule.DeductionAmount);
            loanSchedule.DeductionPercentage = AccuMath.CommercialRound(loanSchedule.DeductionPercentage);
            loanSchedule.TotalPayPeriod = AccuMath.CommercialRound(loanSchedule.TotalPayPeriod);
            loanSchedule.TotalBalanceLeft = AccuMath.CommercialRound(loanSchedule.TotalBalanceLeft);

            if (!GetStatusList().Any(x => x.ToTrimmedLowerCase() == loanSchedule.Status.ToTrimmedLowerCase()))
            {
                if (loanSchedule.TotalBalanceLeft >= loanSchedule.TotalLoanAmount)
                {
                    loanSchedule.Status = LoanSchedule.STATUS_COMPLETE;
                }
                else
                {
                    loanSchedule.Status = LoanSchedule.STATUS_IN_PROGRESS;
                }
            }

            loanSchedule.RecomputeTotalPayPeriod();
            loanSchedule.RecomputePayPeriodLeft();

            // while import loans does not use ViewModel, do this to avoid errors
            var newLoanSchedule = loanSchedule.CloneJson();
            newLoanSchedule.Employee = null;
            // after cloning, TotalPayPeriod and TotalBalanceLeft are not set properly
            // since they are internal. Use a better cloning mechanism next time
            // but for now, recompute those values
            newLoanSchedule.RecomputeTotalPayPeriod();
            newLoanSchedule.RecomputePayPeriodLeft();

            await SaveAsyncFunction(newLoanSchedule);

            if (deferSave == false)
            {
                await _context.SaveChangesAsync();
            }

            // while import loans does not use ViewModel, do this to avoid errors
            loanSchedule.RowID = newLoanSchedule.RowID;
            loanSchedule.TotalPayPeriod = newLoanSchedule.TotalPayPeriod;
            loanSchedule.TotalBalanceLeft = newLoanSchedule.TotalBalanceLeft;
            loanSchedule.Status = newLoanSchedule.Status;
        }

        private async Task SaveAsyncFunction(LoanSchedule loanSchedule)
        {
            // this is the only entity that is checking for int.MinValue,
            // maybe rethink this and check what is causing this
            if (loanSchedule.RowID == null || loanSchedule.RowID == int.MinValue)
                Insert(loanSchedule);
            else
                await UpdateAsync(loanSchedule);
        }

        private void Insert(LoanSchedule loanSchedule)
        {
            loanSchedule.RecomputePayPeriodLeft();

            if (loanSchedule.LoanPayPeriodLeft == 0)
            {
                loanSchedule.Status = LoanSchedule.STATUS_COMPLETE;
            }

            if (loanSchedule.LoanNumber == null)
            {
                loanSchedule.LoanNumber = "";
            }

            loanSchedule.Created = DateTime.Now;

            _context.LoanSchedules.Add(loanSchedule);
        }

        private async Task UpdateAsync(LoanSchedule newLoanSchedule)
        {
            var oldLoanSchedule = await GetByIdAsync(newLoanSchedule.RowID.Value);
            var loanTransactionsCount = await _context.LoanTransactions.
                                            CountAsync(l => l.LoanScheduleID == newLoanSchedule.RowID);

            // if cancelled na yung loan, hindi pwede ma update
            if ((oldLoanSchedule.Status == LoanSchedule.STATUS_CANCELLED))
                throw new BusinessLogicException("Loan schedule is already cancelled!");

            if (newLoanSchedule.TotalBalanceLeft == 0)
            {
                newLoanSchedule.LoanPayPeriodLeft = 0;
                newLoanSchedule.Status = LoanSchedule.STATUS_COMPLETE;
            }

            // if nag start ng magbawas ng loan, dapat hindi na pwede ma edit ang TotalLoanAmount
            if (oldLoanSchedule.TotalBalanceLeft != oldLoanSchedule.TotalLoanAmount || loanTransactionsCount > 0)
            {
                newLoanSchedule.TotalLoanAmount = oldLoanSchedule.TotalLoanAmount;

                // recompute NoOfPayPeriod if TotalLoanAmount changed
                newLoanSchedule.RecomputeTotalPayPeriod();
            }

            if (newLoanSchedule.TotalBalanceLeft > newLoanSchedule.TotalLoanAmount)
            {
                newLoanSchedule.TotalBalanceLeft = oldLoanSchedule.TotalLoanAmount;

                // recompute LoanPayPeriodLeft if TotalBalanceLeft changed
                newLoanSchedule.RecomputePayPeriodLeft();
            }

            _context.Entry(oldLoanSchedule).State = EntityState.Detached;
            _context.Entry(newLoanSchedule).State = EntityState.Modified;
        }

        /// <summary>
        /// 'delete all loans that are not HDMF or SSS. only HDMF or SSS loans are supported in benchmark
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="pagibigLoanId"></param>
        /// <param name="ssLoanId"></param>
        /// <returns></returns>
        public async Task DeleteAllLoansExceptGovernmentLoansAsync(int employeeId,
                                                                int pagibigLoanId,
                                                                int ssLoanId)
        {
            _context.LoanSchedules.
                    RemoveRange(_context.LoanSchedules.
                                        Where(x => x.EmployeeID == employeeId).
                                        Where(x => x.LoanTypeID != pagibigLoanId).
                                        Where(x => x.LoanTypeID != ssLoanId));

            await _context.SaveChangesAsync();
        }

        #endregion CRUD

        #region Queries

        #region Single entity

        public async Task<LoanSchedule> GetByIdAsync(int loanScheduleId)
        {
            return await _context.LoanSchedules
                                .FirstOrDefaultAsync(l => l.RowID == loanScheduleId);
        }

        public async Task<LoanSchedule> GetByIdWithEmployeeAndProductAsync(int id)
        {
            return await _context.LoanSchedules
                                .Include(x => x.Employee)
                                .Include(x => x.LoanType)
                                .FirstOrDefaultAsync(l => l.RowID == id);
        }

        #endregion Single entity

        #region List of entities

        public async Task<IEnumerable<LoanSchedule>> GetByEmployeeAsync(int employeeId)
        {
            return await _context.LoanSchedules.
                        Where(l => l.EmployeeID == employeeId).
                        ToListAsync();
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

        public async Task<IEnumerable<LoanSchedule>> GetActiveLoansByLoanNameAsync(string loanName,
                                                                                    int employeeId)
        {
            return await _context.LoanSchedules.
                Include(l => l.LoanType).
                Include(l => l.LoanType.CategoryEntity).
                Where(l => l.LoanType.CategoryEntity.CategoryName.Trim().ToUpper() == ProductConstant.LOAN_TYPE_CATEGORY.Trim().ToUpper()).
                Where(l => l.LoanType.PartNo.Trim().ToLower() == loanName.ToTrimmedLowerCase()).
                Where(l => l.Status.Trim().ToLower() == LoanSchedule.STATUS_IN_PROGRESS.ToTrimmedLowerCase()).
                Where(l => l.EmployeeID == employeeId).
                ToListAsync();
        }

        public async Task<IEnumerable<LoanTransaction>> GetLoanTransactionsWithPayPeriodAsync(int loanScheduleId)
        {
            return await _context.LoanTransactions.
                            Include(l => l.PayPeriod).
                            Where(l => l.LoanScheduleID == loanScheduleId).
                            ToListAsync();
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
        public async Task<IEnumerable<LoanSchedule>> GetCurrentPayrollLoansAsync(int organizationId,
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
            var loans = await _context.LoanSchedules.
                        Where(l => l.OrganizationID == organizationId).
                        Where(l => l.DedEffectiveDateFrom <= payPeriod.PayToDate).
                        Where(l => acceptedLoans.Contains(l.DeductionSchedule.Trim().ToUpper())).
                        Where(l => l.BonusID == null).
                        ToListAsync();

            var currentLoans = new List<LoanSchedule>();

            // get IN PROGRESS loans
            var inProgressLoans = loans.Where(x => x.Status == LoanSchedule.STATUS_IN_PROGRESS);
            currentLoans.AddRange(inProgressLoans);

            // get not IN PROGRESS loans but has loantransactions for this payperiod
            // (probably was completed this pay period or the loan was edited as
            // ON HOLD or CANCELLED to exclude that loan this pay period)
            var notInProgressLoans = loans.Where(x => x.Status != LoanSchedule.STATUS_IN_PROGRESS);
            var currentLoanTransactions = paystubs.SelectMany(x => x.LoanTransactions);
            var notInProgressLoansWithTransaction = notInProgressLoans.
                                                            Where(x => currentLoanTransactions.
                                                                    Any(t => t.LoanScheduleID == x.RowID));

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

        #region Private helper methods

        private async Task ValidationForBenchmark(LoanSchedule loanSchedule)
        {
            if (loanSchedule == null)
                throw new BusinessLogicException("Invalid loan.");

            if (_systemOwnerService.GetCurrentSystemOwner() == SystemOwnerService.Benchmark)
            {
                if (loanSchedule.EmployeeID == null)
                    throw new BusinessLogicException("Employee does not exists.");

                // IF benchmark
                // #1. Only Pagibig loan or SSS loan can be saved
                // #2. Only one active Pagibig or SSS loan is allowed.

                // #1
                if (loanSchedule.LoanName != ProductConstant.PAG_IBIG_LOAN &&
                    loanSchedule.LoanName != ProductConstant.SSS_LOAN)
                    throw new BusinessLogicException("Only PAGIBIG and SSS loan are allowed!");

                // #2
                if (loanSchedule.Status == LoanSchedule.STATUS_IN_PROGRESS)
                {
                    var sameActiveLoans = await GetActiveLoansByLoanNameAsync(loanSchedule.LoanName,
                                                                                loanSchedule.EmployeeID.Value);

                    // if insert, check if there are any sameActiveLoans
                    // if update, check if there are any sameActiveLoans that is not the currently updated loan schedule
                    if ((loanSchedule.RowID == null && sameActiveLoans.Any()) ||
                        (loanSchedule.RowID.HasValue &&
                            sameActiveLoans.Where(l => l.RowID != loanSchedule.RowID).Any()))
                        throw new BusinessLogicException("Only one active PAGIBIG and one active SSS loan are allowed!");
                }
            }
        }

        #endregion Private helper methods
    }
}