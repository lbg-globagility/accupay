using AccuPay.Data.Entities;
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
        public const string STATUS_IN_PROGRESS = "In Progress";
        public const string STATUS_ON_HOLD = "On hold";
        public const string STATUS_CANCELLED = "Cancelled";
        public const string STATUS_COMPLETE = "Complete";

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

        // TODO: move this to service, shouldn't pass loanTypes in repository
        public async Task SaveManyAsync(List<LoanSchedule> loanSchedules, IEnumerable<Product> loanTypes)
        {
            foreach (var loanSchedule in loanSchedules)
            {
                await SaveWithContextAsync(loanSchedule, loanTypes);

                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveAsync(LoanSchedule loanSchedule, IEnumerable<Product> loanTypes)
        {
            await SaveWithContextAsync(loanSchedule, loanTypes, deferSave: false);
        }

        private async Task SaveWithContextAsync(LoanSchedule loanSchedule,
                                            IEnumerable<Product> loanTypes,
                                            bool deferSave = true)
        {
            // if completed yung loan, hindi pwede ma i-insert or update
            if (loanSchedule.Status == STATUS_COMPLETE)
                throw new ArgumentException("Loan schedule is already completed!");

            if (string.IsNullOrWhiteSpace(loanSchedule.LoanName))
            {
                var loanName = loanTypes.FirstOrDefault(l => l.RowID == loanSchedule.LoanTypeID)?.PartNo;

                loanSchedule.LoanName = loanName;
            }

            await ValidationForBenchmark(loanSchedule);

            // sanitize columns
            loanSchedule.TotalLoanAmount = AccuMath.CommercialRound(loanSchedule.TotalLoanAmount);
            loanSchedule.TotalBalanceLeft = AccuMath.CommercialRound(loanSchedule.TotalBalanceLeft);
            loanSchedule.DeductionAmount = AccuMath.CommercialRound(loanSchedule.DeductionAmount);
            loanSchedule.DeductionPercentage = AccuMath.CommercialRound(loanSchedule.DeductionPercentage);

            loanSchedule.NoOfPayPeriod = AccuMath.CommercialRound(loanSchedule.NoOfPayPeriod);
            loanSchedule.LoanPayPeriodLeft = Convert.ToInt32(
                                                    AccuMath.CommercialRound(
                                                        (decimal)ObjectUtils.ToInteger(
                                                                    loanSchedule.LoanPayPeriodLeft)));

            // while import loans does not use ViewModel, do this to avoid errors
            var newLoanSchedule = loanSchedule.CloneJson();
            newLoanSchedule.Employee = null;

            // add or update the loanSchedule
            if (deferSave == false)
            {
                // this is the only entity that is checking for int.MinValue,
                // maybe rethink this and check what is causing this
                await SaveAsyncFunction(newLoanSchedule);
                await _context.SaveChangesAsync();
            }
            else
            {
                await SaveAsyncFunction(newLoanSchedule);
            }

            // while import loans does not use ViewModel, do this to avoid errors
            loanSchedule.RowID = newLoanSchedule.RowID;
        }

        private async Task SaveAsyncFunction(LoanSchedule loanSchedule)
        {
            if (loanSchedule.RowID == null || loanSchedule.RowID == int.MinValue)
                Insert(loanSchedule);
            else
                await UpdateAsync(loanSchedule);
        }

        private void Insert(LoanSchedule loanSchedule)
        {
            loanSchedule.LoanPayPeriodLeft = ComputeNumberOfPayPeriod(loanSchedule.TotalBalanceLeft,
                                                                    loanSchedule.DeductionAmount);

            if (loanSchedule.LoanPayPeriodLeft < 1)
                loanSchedule.Status = STATUS_COMPLETE;

            if (loanSchedule.LoanNumber == null)
                loanSchedule.LoanNumber = "";

            loanSchedule.Created = DateTime.Now;

            _context.LoanSchedules.Add(loanSchedule);
        }

        private async Task UpdateAsync(LoanSchedule newLoanSchedule)
        {
            var oldLoanSchedule = await GetByIdAsync(newLoanSchedule.RowID.Value);
            var loanTransactionsCount = await _context.LoanTransactions.
                                            CountAsync(l => l.LoanScheduleID == newLoanSchedule.RowID);

            // if cancelled na yung loan, hindi pwede ma update
            if ((oldLoanSchedule.Status == STATUS_CANCELLED))
                throw new ArgumentException("Loan schedule is already cancelled!");

            if (newLoanSchedule.TotalBalanceLeft == 0)
            {
                newLoanSchedule.LoanPayPeriodLeft = 0;
                newLoanSchedule.Status = STATUS_COMPLETE;
            }

            // if nag start ng magbawas ng loan, dapat hindi na pwede ma edit ang TotalLoanAmount
            if (oldLoanSchedule.TotalBalanceLeft != oldLoanSchedule.TotalLoanAmount || loanTransactionsCount > 0)
            {
                newLoanSchedule.TotalLoanAmount = oldLoanSchedule.TotalLoanAmount;

                // recompute NoOfPayPeriod if TotalLoanAmount changed
                newLoanSchedule.NoOfPayPeriod = ComputeNumberOfPayPeriod(newLoanSchedule.TotalLoanAmount, newLoanSchedule.DeductionAmount);
            }

            if (newLoanSchedule.TotalBalanceLeft > newLoanSchedule.TotalLoanAmount)
            {
                newLoanSchedule.TotalBalanceLeft = oldLoanSchedule.TotalLoanAmount;
                // recompute LoanPayPeriodLeft if TotalBalanceLeft changed

                newLoanSchedule.LoanPayPeriodLeft = ComputeNumberOfPayPeriod(newLoanSchedule.TotalBalanceLeft, newLoanSchedule.DeductionAmount);
            }

            _context.LoanSchedules.Attach(newLoanSchedule);
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
            return await _context.LoanSchedules.
                            FirstOrDefaultAsync(l => l.RowID == loanScheduleId);
        }

        #endregion Single entity

        #region List of entities

        public async Task<IEnumerable<LoanSchedule>> GetByEmployeeAsync(int employeeId)
        {
            return await _context.LoanSchedules.
                        Where(l => l.EmployeeID == employeeId).
                        ToListAsync();
        }

        public async Task<IEnumerable<LoanSchedule>> GetActiveLoansByLoanNameAsync(string loanName,
                                                                                    int employeeId)
        {
            return await _context.LoanSchedules.
                Include(l => l.LoanType).
                Include(l => l.LoanType.CategoryEntity).
                Where(l => l.LoanType.CategoryEntity.CategoryName.Trim().ToUpper() == ProductConstant.LOAN_TYPE_CATEGORY.Trim().ToUpper()).
                Where(l => l.LoanType.PartNo.Trim().ToUpper() == loanName.Trim().ToUpper()).
                Where(l => l.Status.Trim().ToUpper() == STATUS_IN_PROGRESS.Trim().ToUpper()).
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

        public async Task<IEnumerable<LoanSchedule>> GetCurrentPayrollLoansAsync(int organizationId,
                                                                                DateTime payPeriodDateTo)
        {
            return await _context.LoanSchedules.
                        Where(l => l.OrganizationID == organizationId).
                        Where(l => l.DedEffectiveDateFrom <= payPeriodDateTo).
                        Where(l => l.Status.Trim().ToUpper() == STATUS_IN_PROGRESS.Trim().ToUpper()).
                        Where(l => l.BonusID == null).
                        ToListAsync();
        }

        #endregion List of entities

        #region Others

        public List<string> GetStatusList()
        {
            return new List<string>()
            {
                STATUS_IN_PROGRESS,
                STATUS_ON_HOLD,
                STATUS_CANCELLED,
                STATUS_COMPLETE
            };
        }

        public int ComputeNumberOfPayPeriod(decimal totalLoanAmount, decimal deductionAmount)
        {
            if (deductionAmount == 0)
                return 0;

            if (deductionAmount > totalLoanAmount)
                return 1;

            return Convert.ToInt32(Math.Ceiling(totalLoanAmount / deductionAmount));
        }

        #endregion Others

        #endregion Queries

        #region Private helper methods

        private async Task ValidationForBenchmark(LoanSchedule loanSchedule)
        {
            if (loanSchedule == null)
                throw new ArgumentException("Invalid loan.");

            if (_systemOwnerService.GetCurrentSystemOwner() == SystemOwnerService.Benchmark)
            {
                if (loanSchedule.EmployeeID == null)
                    throw new ArgumentException("Employee does not exists.");

                // IF benchmark
                // #1. Only Pagibig loan or SSS loan can be saved
                // #2. Only one active Pagibig or SSS loan is allowed.

                // #1
                if (loanSchedule.LoanName != ProductConstant.PAG_IBIG_LOAN &&
                    loanSchedule.LoanName != ProductConstant.SSS_LOAN)
                    throw new ArgumentException("Only PAGIBIG and SSS loan are allowed!");

                // #2
                if (loanSchedule.Status == STATUS_IN_PROGRESS)
                {
                    var sameActiveLoans = await GetActiveLoansByLoanNameAsync(loanSchedule.LoanName,
                                                                                loanSchedule.EmployeeID.Value);

                    // if insert, check if there are any sameActiveLoans
                    // if update, check if there are any sameActiveLoans that is not the currently updated loan schedule
                    if ((loanSchedule.RowID == null && sameActiveLoans.Any()) ||
                        (loanSchedule.RowID.HasValue &&
                            sameActiveLoans.Where(l => l.RowID != loanSchedule.RowID).Any()))
                        throw new ArgumentException("Only one active PAGIBIG and one active SSS loan are allowed!");
                }
            }
        }

        #endregion Private helper methods
    }
}