using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Utilities;
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

        public async Task<LoanSchedule> GetByIdAsync(int loanScheduleId)
        {
            using (var context = new PayrollContext())
            {
                return await context.LoanSchedules.
                                FirstOrDefaultAsync(l => l.RowID == loanScheduleId);
            }
        }

        public async Task<IEnumerable<LoanSchedule>> GetByEmployeeAsync(int employeeId)
        {
            using (var context = new PayrollContext())
            {
                return await context.LoanSchedules.
                            Where(l => l.EmployeeID.Value == employeeId).
                            ToListAsync();
            }
        }

        public async Task<IEnumerable<LoanSchedule>> GetActiveLoansByLoanNameAsync(string loanName,
                                                                                    int employeeId)
        {
            using (var context = new PayrollContext())
            {
                return await context.LoanSchedules.
                    Include(l => l.LoanType).
                    Include(l => l.LoanType.CategoryEntity).
                    Where(l => l.LoanType.CategoryEntity.CategoryName == ProductConstant.LOAN_TYPE_CATEGORY).
                    Where(l => l.LoanType.PartNo.ToUpper() == loanName.ToUpper()).
                    Where(l => l.Status == STATUS_IN_PROGRESS).
                    Where(l => l.EmployeeID.Value == employeeId).
                    ToListAsync();
            }
        }

        public async Task<IEnumerable<LoanTransaction>> GetLoanTransactionsWithPayPeriodAsync(int loanScheduleId)
        {
            using (var context = new PayrollContext())
            {
                return await context.LoanTransactions.
                                Include(l => l.PayPeriod).
                                Where(l => l.LoanScheduleID == loanScheduleId).
                                ToListAsync();
            }
        }

        public async Task<IEnumerable<LoanSchedule>> GetCurrentPayrollLoansAsync(int organizationId,
                                                                                DateTime payPeriodDateTo)
        {
            using (var context = new PayrollContext())
            {
                return await context.LoanSchedules.
                            Where(l => l.OrganizationID == organizationId).
                            Where(l => l.DedEffectiveDateFrom <= payPeriodDateTo).
                            Where(l => l.Status == STATUS_IN_PROGRESS).
                            Where(l => l.BonusID == null).
                            ToListAsync();
            }
        }

        public int ComputeNumberOfPayPeriod(decimal totalLoanAmount, decimal deductionAmount)
        {
            if (deductionAmount == 0)
                return 0;

            if (deductionAmount > totalLoanAmount)
                return 1;

            return Convert.ToInt32(Math.Ceiling(totalLoanAmount / deductionAmount));
        }

        #region CRUD

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
            using (var context = new PayrollContext())
            {
                context.LoanSchedules.
                        RemoveRange(context.LoanSchedules.
                                            Where(x => x.EmployeeID == employeeId).
                                            Where(x => x.LoanTypeID != pagibigLoanId).
                                            Where(x => x.LoanTypeID != ssLoanId));

                await context.SaveChangesAsync();
            }
        }

        public async Task SaveManyAsync(List<LoanSchedule> currentLoanSchedules,
                                        IEnumerable<Product> loanTypes,
                                        int organizationId,
                                        int userId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                foreach (var loanSchedule in currentLoanSchedules)
                {
                    await InternalSaveAsync(loanSchedule,
                                    loanTypes,
                                    organizationId: organizationId,
                                    userId: userId,
                                    passedContext: context);

                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task SaveAsync(LoanSchedule loanSchedule,
                                    IEnumerable<Product> loanTypes,
                                    int organizationId,
                                    int userId)
        {
            await InternalSaveAsync(loanSchedule: loanSchedule,
                                    loanTypes: loanTypes,
                                    organizationId: organizationId,
                                    userId: userId);
        }

        internal async Task InternalSaveAsync(LoanSchedule loanSchedule,
                                            IEnumerable<Product> loanTypes,
                                            int organizationId,
                                            int userId,
                                            PayrollContext passedContext = null/* TODO Change to default(_) if this is not a reference type */)
        {
            // if completed yung loan, hindi pwede ma i-insert or update
            if (loanSchedule.Status == STATUS_COMPLETE)
                throw new ArgumentException("Loan schedule is already completed!");

            if (string.IsNullOrWhiteSpace(loanSchedule.LoanName))
            {
                var loanName = loanTypes.FirstOrDefault(l => l.RowID.Value == loanSchedule.LoanTypeID.Value)?.PartNo;

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

            loanSchedule.OrganizationID = organizationId;

            // add or update the loanSchedule
            if (passedContext == null)
            {
                using (PayrollContext newContext = new PayrollContext())
                {
                    if (loanSchedule.RowID == null || loanSchedule.RowID == int.MinValue)
                        this.Insert(loanSchedule, newContext, userId);
                    else
                        await this.UpdateAsync(loanSchedule, newContext, userId);

                    await newContext.SaveChangesAsync();
                }
            }
            else if (loanSchedule.RowID == null || loanSchedule.RowID == int.MinValue)
                this.Insert(loanSchedule, passedContext, userId);
            else
                await this.UpdateAsync(loanSchedule, passedContext, userId);
        }

        private async Task ValidationForBenchmark(LoanSchedule loanSchedule)
        {
            if (loanSchedule == null)
                throw new ArgumentException("Invalid loan.");

            var sys_ownr = new SystemOwnerService();
            if (sys_ownr.CurrentSystemOwner == SystemOwnerService.Benchmark)
            {
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
                            sameActiveLoans.Where(l => l.RowID.Value != loanSchedule.RowID.Value).Any()))
                        throw new ArgumentException("Only one active PAGIBIG and one active SSS loan are allowed!");
                }
            }
        }

        public async Task DeleteAsync(int loanScheduleId)
        {
            using (var context = new PayrollContext())
            {
                var loanSchedule = await GetByIdAsync(loanScheduleId);

                context.Remove(loanSchedule);

                await context.SaveChangesAsync();
            }
        }

        private void Insert(LoanSchedule loanSchedule, PayrollContext context, int userId)
        {
            loanSchedule.LoanPayPeriodLeft = ComputeNumberOfPayPeriod(loanSchedule.TotalBalanceLeft, loanSchedule.DeductionAmount);

            if (loanSchedule.LoanPayPeriodLeft < 1)
                loanSchedule.Status = STATUS_COMPLETE;

            if (loanSchedule.LoanNumber == null)
                loanSchedule.LoanNumber = "";

            loanSchedule.Created = DateTime.Now;
            loanSchedule.CreatedBy = userId;

            context.LoanSchedules.Add(loanSchedule);
        }

        private async Task UpdateAsync(LoanSchedule newLoanSchedule, PayrollContext context, int userId)
        {
            var oldLoanSchedule = await this.GetByIdAsync(newLoanSchedule.RowID.Value);
            var loanTransactionsCount = await context.LoanTransactions.CountAsync(l => l.LoanScheduleID == newLoanSchedule.RowID.Value);

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

            newLoanSchedule.LastUpdBy = userId;

            context.LoanSchedules.Attach(newLoanSchedule);
            context.Entry(newLoanSchedule).State = EntityState.Modified;
        }
    }

    #endregion CRUD
}