using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Utilities;
using AccuPay.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class LoanDataService : BaseEmployeeDataService<LoanSchedule>
    {
        private const string UserActivityName = "Loan";

        private readonly ListOfValueRepository _listOfValueRepository;
        private readonly LoanRepository _loanRepository;
        private readonly ProductRepository _productRepository;

        private readonly SystemOwnerService _systemOwnerService;

        public LoanDataService(
            LoanRepository loanRepository,
            ListOfValueRepository listOfValueRepository,
            PayPeriodRepository payPeriodRepository,
            ProductRepository productRepository,
            UserActivityRepository userActivityRepository,
            SystemOwnerService systemOwnerService,
            PayrollContext context,
            PolicyHelper policy) :

            base(loanRepository,
                payPeriodRepository,
                userActivityRepository,
                context,
                policy,
                entityName: "Loan")
        {
            _loanRepository = loanRepository;
            _listOfValueRepository = listOfValueRepository;
            _productRepository = productRepository;
            _systemOwnerService = systemOwnerService;
        }

        #region Save

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

        public async Task BatchApply(IReadOnlyCollection<Imports.Loans.LoanImportModel> validRecords, int organizationId, int userId)
        {
            var loanWithLoanTypeNotYetExists = validRecords
                .Where(v => v.LoanTypeNotExists)
                .GroupBy(v => v.LoanName)
                .Select(v => v.FirstOrDefault().LoanName)
                .ToList();

            if (loanWithLoanTypeNotYetExists.Any())
            {
                var newlyAddedLoanTypes = await _productRepository
                    .AddManyLoanTypeAsync(loanWithLoanTypeNotYetExists, organizationId: organizationId, userId: userId);

                var validRecordsWithLoanTypeNotYetExists = validRecords.Where(v => v.LoanTypeNotExists).ToList();
                foreach (var validRecord in validRecordsWithLoanTypeNotYetExists)
                {
                    var newlyAddedLoanType = newlyAddedLoanTypes.Where(v => v.PartNo == validRecord.LoanName).FirstOrDefault();
                    validRecord.LoanTypeId = newlyAddedLoanType.RowID.Value;
                }
            }

            List<LoanSchedule> loanSchedules = new List<LoanSchedule>();
            foreach (var validRecord in validRecords)
            {
                var loanSchedule = new LoanSchedule()
                {
                    Comments = validRecord.Comments,
                    CreatedBy = userId,
                    DedEffectiveDateFrom = validRecord.StartDate,
                    DeductionAmount = validRecord.DeductionAmount,
                    DeductionSchedule = validRecord.DeductionSchedule,
                    EmployeeID = validRecord.EmployeeId,
                    LoanName = validRecord.LoanName,
                    LoanNumber = validRecord.LoanNumber,
                    LoanTypeID = validRecord.LoanTypeId,
                    OrganizationID = organizationId,
                    TotalBalanceLeft = validRecord.TotalLoanBalance,
                    TotalLoanAmount = validRecord.TotalLoanAmount,
                    Status = LoanSchedule.STATUS_IN_PROGRESS
                };

                loanSchedules.Add(loanSchedule);
            }

            await _loanRepository.SaveManyAsync(loanSchedules);
        }

        #endregion Save

        #region Overrides

        protected override string GetUserActivityName(LoanSchedule loan) => UserActivityName;

        protected override string CreateUserActivitySuffixIdentifier(LoanSchedule loan) =>
            $" with type '{loan.LoanName}' and start date '{loan.DedEffectiveDateFrom.ToShortDateString()}'";

        protected override async Task SanitizeEntity(LoanSchedule loan, LoanSchedule oldLoan)
        {
            await base.SanitizeEntity(entity: loan, oldEntity: oldLoan);

            Validate(loan, oldLoan);

            await SanitizeProperties(loan);

            if (loan.IsNewEntity)
                SanitizeInsert(loan);
            else
                SanitizeUpdate(loan, oldLoan);
        }

        protected override async Task AdditionalDeleteValidation(LoanSchedule loan)
        {
            if (loan.Status == LoanSchedule.STATUS_COMPLETE)
                throw new BusinessLogicException("Loan is already completed!");

            if ((await _loanRepository.GetLoanTransactionsWithPayPeriodAsync(loan.RowID.Value)).Count > 0)
                throw new BusinessLogicException("This loan has already started and therefore cannot be deleted. Try changing its Status to \"On Hold\" or \"Cancelled\" instead.");

            await Task.CompletedTask;
        }

        protected override async Task AdditionalSaveValidation(LoanSchedule loan, LoanSchedule oldLoan)
        {
            // validate start date should not be in a Closed Payroll
            if (CheckIfStartDateNeedsToBeValidated(new List<LoanSchedule>() { oldLoan }, loan))
            {
                await CheckIfDataIsWithinClosedPayPeriod(loan.DedEffectiveDateFrom, loan.OrganizationID.Value);
            }

            // validate deduction schedules
            var deductionSchedules = _listOfValueRepository
                .ConvertToStringList(await _listOfValueRepository.GetDeductionSchedulesAsync())
                .Select(x => x.ToTrimmedLowerCase());

            if (deductionSchedules.Contains(loan.DeductionSchedule.ToTrimmedLowerCase()) == false)
                throw new BusinessLogicException("Deduction schedule is not valid.");

            // sanitize Loan Name
            if (string.IsNullOrWhiteSpace(loan.LoanName))
            {
                loan.LoanName = (await _productRepository
                    .GetByIdAsync(loan.LoanTypeID.Value))
                    ?.PartNo;
            }
        }

        protected async override Task AdditionalSaveManyValidation(List<LoanSchedule> loans, List<LoanSchedule> oldLoans, SaveType saveType)
        {
            // validate start date should not be in a Closed Payroll
            int? organizationId = null;
            loans.ForEach(x =>
            {
                organizationId = ValidateOrganization(organizationId, x.OrganizationID);
            });

            var validatableStartDates = loans
                 .Where(loan =>
                 {
                     return CheckIfStartDateNeedsToBeValidated(oldLoans, loan);
                 })
                 .Select(x => x.DedEffectiveDateFrom)
                 .ToArray();

            await CheckIfDataIsWithinClosedPayPeriod(validatableStartDates, organizationId.Value);

            // validate deduction schedules and sanitize Loan Name
            var deductionSchedules = _listOfValueRepository
                .ConvertToStringList(await _listOfValueRepository.GetDeductionSchedulesAsync())
                .Select(x => x.ToTrimmedLowerCase());

            var loanTypeIds = loans
                .Where(x => x.LoanTypeID.HasValue)
                .Select(x => x.LoanTypeID.Value).Distinct()
                .ToArray();
            var loanTypes = await _productRepository.GetManyByIdsAsync(loanTypeIds);

            foreach (var loan in loans)
            {
                if (deductionSchedules.Contains(loan.DeductionSchedule.ToTrimmedLowerCase()) == false)
                    throw new BusinessLogicException("Deduction schedule is not valid.");

                if (string.IsNullOrWhiteSpace(loan.LoanName))
                {
                    loan.LoanName = loanTypes
                        .Where(x => x.RowID == loan.LoanTypeID)
                        .FirstOrDefault()?
                        .PartNo;
                }
            }
        }

        #endregion Overrides

        #region Private Methods

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

        private void SanitizeUpdate(LoanSchedule newLoan, LoanSchedule oldLoan)
        {
            // if cancelled na yung loan, hindi pwede ma update
            if ((oldLoan.Status == LoanSchedule.STATUS_CANCELLED))
                throw new BusinessLogicException("Loan is already cancelled!");

            if (newLoan.TotalBalanceLeft == 0)
            {
                newLoan.LoanPayPeriodLeft = 0;
                newLoan.Status = LoanSchedule.STATUS_COMPLETE;
            }

            // if nag start ng magbawas ng loan, dapat hindi na pwede ma edit ang TotalLoanAmount and Loan Type
            if (oldLoan.HasStarted)// || loanTransactionsCount > 0)
            {
                newLoan.TotalLoanAmount = oldLoan.TotalLoanAmount;
                newLoan.TotalBalanceLeft = oldLoan.TotalBalanceLeft;

                newLoan.LoanTypeID = oldLoan.LoanTypeID;

                // recompute NoOfPayPeriod if TotalLoanAmount changed
                newLoan.RecomputeTotalPayPeriod();
            }

            if (newLoan.TotalBalanceLeft > newLoan.TotalLoanAmount)
            {
                newLoan.TotalBalanceLeft = oldLoan.TotalLoanAmount;

                // recompute LoanPayPeriodLeft if TotalBalanceLeft changed
                newLoan.RecomputePayPeriodLeft();
            }
        }

        private async Task SanitizeProperties(LoanSchedule loan)
        {
            // move this to overriden save validation methods
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

        private static void Validate(LoanSchedule loan, LoanSchedule oldLoan)
        {
            if ((oldLoan?.Status ?? loan.Status) == LoanSchedule.STATUS_COMPLETE)
                throw new BusinessLogicException("Loan is already completed!");

            if (loan.LoanTypeID == null)
                throw new BusinessLogicException("Loan type is required.");

            if (loan.DedEffectiveDateFrom < PayrollTools.SqlServerMinimumDate)
                throw new BusinessLogicException("Date cannot be earlier than January 1, 1753");

            if (loan.TotalLoanAmount < 0)
                throw new BusinessLogicException("Total loan amount cannot be less than 0.");

            if (loan.DeductionAmount < 0)
                throw new BusinessLogicException("Deduction amount cannot be less than 0.");

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
                    var sameActiveLoans = await _loanRepository.GetActiveLoansByLoanNameAsync(
                        loan.LoanName,
                        loan.EmployeeID.Value);

                    // if insert, check if there are any sameActiveLoans
                    // if update, check if there are any sameActiveLoans that is not the currently updated loan
                    if ((loan.RowID == null && sameActiveLoans.Any()) ||
                        (loan.RowID.HasValue &&
                            sameActiveLoans.Where(l => l.RowID != loan.RowID).Any()))
                        throw new BusinessLogicException("Only one active PAGIBIG and one active SSS loan are allowed!");
                }
            }
        }

        private bool CheckIfStartDateNeedsToBeValidated(List<LoanSchedule> oldLoans, LoanSchedule loan)
        {
            // either a new loan
            if (loan.IsNewEntity) return true;

            // or an existing loan where its Start Date is to be updated
            var oldLoan = oldLoans.Where(o => o.RowID == loan.RowID).FirstOrDefault();

            if (loan.DedEffectiveDateFrom.ToMinimumHourValue() != oldLoan.DedEffectiveDateFrom.ToMinimumHourValue())
                return true;

            return false;
        }

        #endregion Private Methods
    }
}
