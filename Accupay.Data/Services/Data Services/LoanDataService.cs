using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Utilities;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class LoanDataService : BaseSavableDataService<LoanSchedule>
    {
        private readonly LoanRepository _loanRepository;

        private readonly PayrollContext _context;
        private readonly SystemOwnerService _systemOwnerService;

        private readonly ProductRepository _productRepository;

        public LoanDataService(
            LoanRepository loanRepository,
            PayrollContext context,
            SystemOwnerService systemOwnerService,
            ProductRepository productRepository,
            PayPeriodRepository payPeriodRepository,
            PolicyHelper policy) :

            base(loanRepository,
                payPeriodRepository,
                policy,
                entityDoesNotExistOnDeleteErrorMessage: "Loan does not exists.")
        {
            _loanRepository = loanRepository;
            _context = context;
            _systemOwnerService = systemOwnerService;
            _productRepository = productRepository;
        }

        #region CRUD

        /// <summary>
        /// 'delete all loans that are not HDMF or SSS. only HDMF or SSS loans are supported in benchmark
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="pagibigLoanId"></param>
        /// <param name="ssLoanId"></param>
        /// <returns></returns>
        public async Task DeleteAllLoansExceptGovernmentLoansAsync(
            int employeeId,
            int pagibigLoanId,
            int ssLoanId)
        {
            await _loanRepository.DeleteAllLoansExceptGovernmentLoansAsync(
                employeeId: employeeId,
                pagibigLoanId: pagibigLoanId,
                ssLoanId: ssLoanId);
        }

        protected override async Task SanitizeEntity(LoanSchedule loan)
        {
            Validate(loan);

            await SanitizeProperties(loan);

            if (IsNewEntity(loan.RowID))
                SanitizeInsert(loan);
            else
                await SanitizeUpdate(loan);
        }

        private async Task SanitizeUpdate(LoanSchedule newLoan)
        {
            var oldLoan = await _loanRepository.GetByIdAsync(newLoan.RowID.Value);

            var loanTransactionsCount = await _context.LoanTransactions.
                                            CountAsync(l => l.LoanScheduleID == newLoan.RowID);

            // if cancelled na yung loan, hindi pwede ma update
            if ((oldLoan.Status == LoanSchedule.STATUS_CANCELLED))
                throw new BusinessLogicException("Loan schedule is already cancelled!");

            if (newLoan.TotalBalanceLeft == 0)
            {
                newLoan.LoanPayPeriodLeft = 0;
                newLoan.Status = LoanSchedule.STATUS_COMPLETE;
            }

            // if nag start ng magbawas ng loan, dapat hindi na pwede ma edit ang TotalLoanAmount
            if (oldLoan.TotalBalanceLeft != oldLoan.TotalLoanAmount)// || loanTransactionsCount > 0)
            {
                newLoan.TotalLoanAmount = oldLoan.TotalLoanAmount;
                newLoan.TotalBalanceLeft = oldLoan.TotalBalanceLeft;

                // recompute NoOfPayPeriod if TotalLoanAmount changed
                newLoan.RecomputeTotalPayPeriod();
            }

            if (newLoan.TotalBalanceLeft > newLoan.TotalLoanAmount)
            {
                newLoan.TotalBalanceLeft = oldLoan.TotalLoanAmount;

                // recompute LoanPayPeriodLeft if TotalBalanceLeft changed
                newLoan.RecomputePayPeriodLeft();
            }

            _context.Entry(oldLoan).State = EntityState.Detached;
        }

        private static void SanitizeInsert(LoanSchedule loan)
        {
            if (loan.LoanPayPeriodLeft == 0)
            {
                loan.Status = LoanSchedule.STATUS_COMPLETE;
            }

            if (loan.LoanNumber == null)
            {
                loan.LoanNumber = "";
            }

            loan.Created = DateTime.Now;
        }

        private async Task SanitizeProperties(LoanSchedule loan)
        {
            if (string.IsNullOrWhiteSpace(loan.LoanName))
            {
                var loanName = (await _productRepository
                                    .GetByIdAsync(loan.LoanTypeID.Value))
                                    ?.PartNo;

                loan.LoanName = loanName;
            }

            await ValidationForBenchmark(loan);

            loan.TotalLoanAmount = AccuMath.CommercialRound(loan.TotalLoanAmount);
            loan.DeductionAmount = AccuMath.CommercialRound(loan.DeductionAmount);
            loan.DeductionPercentage = AccuMath.CommercialRound(loan.DeductionPercentage);
            loan.TotalPayPeriod = AccuMath.CommercialRound(loan.TotalPayPeriod);
            loan.TotalBalanceLeft = AccuMath.CommercialRound(loan.TotalBalanceLeft);

            if (!_loanRepository.GetStatusList().Any(x => x.ToTrimmedLowerCase() == loan.Status.ToTrimmedLowerCase()))
            {
                if (loan.TotalBalanceLeft >= loan.TotalLoanAmount)
                {
                    loan.Status = LoanSchedule.STATUS_COMPLETE;
                }
                else
                {
                    loan.Status = LoanSchedule.STATUS_IN_PROGRESS;
                }
            }

            loan.RecomputeTotalPayPeriod();
            loan.RecomputePayPeriodLeft();
        }

        private static void Validate(LoanSchedule loan)
        {
            if (loan.Status == LoanSchedule.STATUS_COMPLETE)
                throw new BusinessLogicException("Loan schedule is already completed!");

            if (loan.OrganizationID == null)
                throw new BusinessLogicException("Organization is required.");

            if (loan.EmployeeID == null)
                throw new BusinessLogicException("Employee is required.");

            if (loan.LoanTypeID == null)
                throw new BusinessLogicException("Loan type is required.");

            if (loan.TotalLoanAmount < 0)
                throw new BusinessLogicException("Total loan amount cannot be less than 0.");

            if (loan.DeductionAmount < 0)
                throw new BusinessLogicException("Deduction amount cannot be less than 0.");

            // maybe check if it is a valid deduction schedule?
            if (string.IsNullOrWhiteSpace(loan.DeductionSchedule))
                throw new BusinessLogicException("Deduction schedule is required.");
        }

        private async Task ValidationForBenchmark(LoanSchedule loan)
        {
            if (loan == null)
                throw new BusinessLogicException("Invalid loan.");

            if (_systemOwnerService.GetCurrentSystemOwner() == SystemOwnerService.Benchmark)
            {
                if (loan.EmployeeID == null)
                    throw new BusinessLogicException("Employee does not exists.");

                // IF benchmark
                // #1. Only Pagibig loan or SSS loan can be saved
                // #2. Only one active Pagibig or SSS loan is allowed.

                // #1
                if (loan.LoanName != ProductConstant.PAG_IBIG_LOAN &&
                    loan.LoanName != ProductConstant.SSS_LOAN)
                    throw new BusinessLogicException("Only PAGIBIG and SSS loan are allowed!");

                // #2
                if (loan.Status == LoanSchedule.STATUS_IN_PROGRESS)
                {
                    var sameActiveLoans = await _loanRepository.GetActiveLoansByLoanNameAsync(loan.LoanName,
                                                                                            loan.EmployeeID.Value);

                    // if insert, check if there are any sameActiveLoans
                    // if update, check if there are any sameActiveLoans that is not the currently updated loan schedule
                    if ((loan.RowID == null && sameActiveLoans.Any()) ||
                        (loan.RowID.HasValue &&
                            sameActiveLoans.Where(l => l.RowID != loan.RowID).Any()))
                        throw new BusinessLogicException("Only one active PAGIBIG and one active SSS loan are allowed!");
                }
            }
        }

        #endregion CRUD
    }
}