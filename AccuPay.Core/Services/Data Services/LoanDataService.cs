using AccuPay.Core.Entities;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Helpers;
using AccuPay.Core.Repositories;
using AccuPay.Utilities;
using AccuPay.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Core.Services
{
    public class LoanDataService : BaseEmployeeDataService<Loan>
    {
        private const string UserActivityName = "Loan";

        private readonly ListOfValueRepository _listOfValueRepository;
        private readonly EmployeeRepository _employeeRepository;
        private readonly LoanRepository _loanRepository;
        private readonly ProductRepository _productRepository;
        private readonly SystemOwnerService _systemOwnerService;

        public LoanDataService(
            EmployeeRepository employeeRepository,
            LoanRepository loanRepository,
            ListOfValueRepository listOfValueRepository,
            PayPeriodRepository payPeriodRepository,
            ProductRepository productRepository,
            UserActivityRepository userActivityRepository,
            SystemOwnerService systemOwnerService,
            PayrollContext context,
            IPolicyHelper policy) :

            base(loanRepository,
                payPeriodRepository,
                userActivityRepository,
                context,
                policy,
                entityName: "Loan")
        {
            _employeeRepository = employeeRepository;
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

        public async Task BatchApply(IReadOnlyCollection<Imports.Loans.LoanImportModel> validRecords, int organizationId, int currentlyLoggedInUserId)
        {
            var loanWithLoanTypeNotYetExists = validRecords
                .Where(x => x.LoanTypeNotExists)
                .GroupBy(x => x.LoanName)
                .Select(x => x.FirstOrDefault().LoanName)
                .ToList();

            if (loanWithLoanTypeNotYetExists.Any())
            {
                var newlyAddedLoanTypes = await _productRepository
                    .AddManyLoanTypeAsync(loanWithLoanTypeNotYetExists, organizationId: organizationId, userId: currentlyLoggedInUserId);

                var validRecordsWithLoanTypeNotYetExists = validRecords.Where(v => v.LoanTypeNotExists).ToList();
                foreach (var validRecord in validRecordsWithLoanTypeNotYetExists)
                {
                    var newlyAddedLoanType = newlyAddedLoanTypes.Where(v => v.PartNo == validRecord.LoanName).FirstOrDefault();
                    validRecord.LoanTypeId = newlyAddedLoanType.RowID.Value;
                }
            }

            List<Loan> loans = new List<Loan>();
            foreach (var validRecord in validRecords)
            {
                var loan = new Loan()
                {
                    Comments = validRecord.Comments,
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
                    Status = Loan.STATUS_IN_PROGRESS
                };

                loans.Add(loan);
            }

            await SaveManyAsync(loans, currentlyLoggedInUserId);
        }

        #endregion Save

        #region Overrides

        protected override string GetUserActivityName(Loan loan)
        {
            return UserActivityName;
        }

        protected override string CreateUserActivitySuffixIdentifier(Loan loan)
        {
            return $" with type '{loan.LoanType?.PartNo}' and start date '{loan.DedEffectiveDateFrom.ToShortDateString()}'";
        }

        protected override async Task SanitizeEntity(Loan loan, Loan oldLoan, int currentlyLoggedInUserId)
        {
            await base.SanitizeEntity(
                entity: loan,
                oldEntity: oldLoan,
                currentlyLoggedInUserId: currentlyLoggedInUserId);

            Validate(loan, oldLoan);

            SanitizeProperties(loan);

            if (loan.IsNewEntity)
            {
                SanitizeInsert(loan);
            }
            else
            {
                SanitizeUpdate(loan, oldLoan);
            }
        }

        protected override async Task AdditionalDeleteValidation(Loan loan)
        {
            if (loan.Status == Loan.STATUS_COMPLETE)
                throw new BusinessLogicException("Loan is already completed!");

            if ((await _loanRepository.GetLoanTransactionsWithPayPeriodAsync(loan.RowID.Value)).Count > 0)
                throw new BusinessLogicException("This loan has already started and therefore cannot be deleted. Try changing its Status to \"On Hold\" or \"Cancelled\" instead.");

            await Task.CompletedTask;
        }

        protected override async Task AdditionalSaveValidation(Loan loan, Loan oldLoan)
        {
            await ValidationForBenchmark(loan);

            if (!loan.IsNewEntity)
            {
                var loanTransactionList = await _loanRepository.GetLoanTransactionsAsync(PageOptions.AllData, loan.RowID.Value);

                ValidateTotalAmountAndBalance(loan, oldLoan, loanTransactionList.Items);
            }

            // validate start date should not be in a Closed Payroll
            if (CheckIfStartDateNeedsToBeValidated(new List<Loan>() { oldLoan }, loan))
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

            // sanitize Basic Monthly Salary
            if (loan.IsNewEntity)
            {
                await AddBasicMonthlySalaryData(loan);
            }
        }

        protected async override Task AdditionalSaveManyValidation(List<Loan> loans, List<Loan> oldLoans, SaveType saveType)
        {
            var updatedLoanIds = loans
                .Where(x => !x.IsNewEntity)
                .Select(x => x.RowID.Value)
                .ToArray();

            var allLoanTransactionList = await _loanRepository.GetLoanTransactionsAsync(PageOptions.AllData, updatedLoanIds);

            foreach (var loan in loans)
            {
                await ValidationForBenchmark(loan);

                if (!loan.IsNewEntity)
                {
                    var oldLoan = oldLoans.Where(x => x.RowID == loan.RowID).FirstOrDefault();

                    var loanTransactions = allLoanTransactionList.Items.Where(x => x.LoanID == loan.RowID);

                    ValidateTotalAmountAndBalance(loan, oldLoan, loanTransactions);
                }
            }

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

            // validate deduction schedules then sanitize Loan Name and Basic Monthly Salary
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

                if (loan.IsNewEntity)
                {
                    await AddBasicMonthlySalaryData(loan);
                }
            }
        }

        protected override async Task PostDeleteAction(Loan entity, int currentlyLoggedInUserId)
        {
            // supplying LoanType data for saving useractivity
            entity.LoanType = await _productRepository.GetByIdAsync(entity.LoanTypeID.Value);

            await base.PostDeleteAction(entity, currentlyLoggedInUserId);
        }

        protected override async Task PostSaveAction(Loan entity, Loan oldEntity, SaveType saveType)
        {
            // supplying LoanType data for saving useractivity
            entity.LoanType = await _productRepository.GetByIdAsync(entity.LoanTypeID.Value);

            if (oldEntity != null)
            {
                oldEntity.LoanType = await _productRepository.GetByIdAsync(oldEntity.LoanTypeID.Value);
            }

            await base.PostSaveAction(entity, oldEntity, saveType);
        }

        protected override async Task PostSaveManyAction(
            IReadOnlyCollection<Loan> entities,
            IReadOnlyCollection<Loan> oldEntities,
            SaveType saveType,
            int currentlyLoggedInUserId)
        {
            if (!entities.Any()) return;

            // supplying LoanType data for saving useractivity
            var allowanceTypeIds = entities.Select(x => x.LoanTypeID.Value).ToList();
            allowanceTypeIds.AddRange(oldEntities.Select(x => x.LoanTypeID.Value).ToList());

            allowanceTypeIds = allowanceTypeIds.Distinct().ToList();

            var allowanceTypes = await _productRepository.GetManyByIdsAsync(allowanceTypeIds.ToArray());

            foreach (var entity in entities)
            {
                entity.LoanType = allowanceTypes.Where(x => x.RowID.Value == entity.LoanTypeID).FirstOrDefault();
            }

            foreach (var entity in oldEntities)
            {
                entity.LoanType = allowanceTypes.Where(x => x.RowID.Value == entity.LoanTypeID).FirstOrDefault();
            }

            await base.PostSaveManyAction(entities, oldEntities, saveType, currentlyLoggedInUserId);
        }

        protected async override Task RecordUpdate(Loan newValue, Loan oldValue)
        {
            if (oldValue == null) return;

            var changes = new List<UserActivityItem>();
            var entityName = UserActivityName.ToLower();

            var suffixIdentifier = $"of {entityName}{CreateUserActivitySuffixIdentifier(oldValue)}.";

            if (newValue.LoanType?.PartNo != oldValue.LoanType?.PartNo)
            {
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated type from '{oldValue.LoanType?.PartNo}' to '{newValue.LoanType?.PartNo}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            }
            if (newValue.LoanNumber != oldValue.LoanNumber)
            {
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated loan number from '{oldValue.LoanNumber}' to '{newValue.LoanNumber}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            }
            if (newValue.TotalLoanAmount != oldValue.TotalLoanAmount)
            {
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated total amount from '{oldValue.TotalLoanAmount}' to '{newValue.TotalLoanAmount}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            }
            if (newValue.DedEffectiveDateFrom != oldValue.DedEffectiveDateFrom)
            {
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated start date '{oldValue.DedEffectiveDateFrom.ToShortDateString()}' to '{newValue.DedEffectiveDateFrom.ToShortDateString()}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            }
            if (newValue.DeductionAmount != oldValue.DeductionAmount)
            {
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated deduction amount from '{oldValue.DeductionAmount}' to '{newValue.DeductionAmount}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            }
            if (newValue.Status != oldValue.Status)
            {
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated status from '{oldValue.Status}' to '{newValue.Status}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            }
            if (newValue.DeductionPercentage != oldValue.DeductionPercentage)
            {
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated interest percentage from '{oldValue.DeductionPercentage}' to '{newValue.DeductionPercentage}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            }
            if (newValue.DeductionSchedule != oldValue.DeductionSchedule)
            {
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated deduction schedule from '{oldValue.DeductionSchedule}' to '{newValue.DeductionSchedule}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            }
            if (newValue.Comments != oldValue.Comments)
            {
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated comments from '{oldValue.Comments}' to '{newValue.Comments}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            }

            if (changes.Any())
            {
                await _userActivityRepository.CreateRecordAsync(
                    newValue.LastUpdBy.Value,
                    UserActivityName,
                    newValue.OrganizationID.Value,
                    UserActivityRepository.RecordTypeEdit,
                    changes);
            }
        }

        #endregion Overrides

        #region Private Methods

        private static void SanitizeInsert(Loan loan)
        {
            if (loan.LoanPayPeriodLeft == 0)
            {
                loan.Status = Loan.STATUS_COMPLETE;
            }

            if (loan.LoanNumber == null)
            {
                loan.LoanNumber = "";
            }

            loan.TotalBalanceLeft = loan.TotalLoanAmount;
            loan.OriginalDeductionAmount = loan.DeductionAmount;
        }

        private void SanitizeUpdate(Loan newLoan, Loan oldLoan)
        {
            // if cancelled na yung loan, hindi pwede ma update
            if ((oldLoan.Status == Loan.STATUS_CANCELLED))
                throw new BusinessLogicException("Loan is already cancelled!");

            if (newLoan.TotalBalanceLeft == 0)
            {
                newLoan.LoanPayPeriodLeft = 0;
                newLoan.Status = Loan.STATUS_COMPLETE;
            }

            if (newLoan.TotalBalanceLeft > newLoan.TotalLoanAmount)
            {
                newLoan.TotalBalanceLeft = oldLoan.TotalLoanAmount;

                // recompute LoanPayPeriodLeft if TotalBalanceLeft changed
                newLoan.RecomputePayPeriodLeft();
            }
        }

        private void ValidateTotalAmountAndBalance(Loan newLoan, Loan oldLoan, IEnumerable<LoanTransaction> loanTransactions)
        {
            if (newLoan.IsNewEntity || oldLoan == null) return;

            // BasicMonthlySalary should never changed even if the current salary changed.
            // Maybe recompute this if the loan has not started yet.
            newLoan.BasicMonthlySalary = oldLoan.BasicMonthlySalary;

            // if nag start ng magbawas ng loan, dapat hindi na pwede ma edit ang TotalLoanAmount, OriginalDeductionAmount and Loan Type
            if (loanTransactions.Any())
            {
                newLoan.TotalLoanAmount = oldLoan.TotalLoanAmount;
                newLoan.TotalBalanceLeft = oldLoan.TotalBalanceLeft;

                newLoan.LoanTypeID = oldLoan.LoanTypeID;

                newLoan.OriginalDeductionAmount = oldLoan.OriginalDeductionAmount;

                newLoan.RecomputeTotalPayPeriod();
                newLoan.RecomputePayPeriodLeft();
            }
            else
            {
                newLoan.OriginalDeductionAmount = newLoan.DeductionAmount;

                // else Balance should always be equals to Loan Amount
                newLoan.TotalBalanceLeft = newLoan.TotalLoanAmount;
                newLoan.RecomputePayPeriodLeft();
            }
        }

        private void SanitizeProperties(Loan loan)
        {
            loan.TotalLoanAmount = AccuMath.CommercialRound(loan.TotalLoanAmount);
            loan.DeductionAmount = AccuMath.CommercialRound(loan.DeductionAmount);
            loan.DeductionPercentage = AccuMath.CommercialRound(loan.DeductionPercentage);
            loan.TotalBalanceLeft = AccuMath.CommercialRound(loan.TotalBalanceLeft);

            loan.RecomputeTotalPayPeriod();
            loan.RecomputePayPeriodLeft();
        }

        private void Validate(Loan loan, Loan oldLoan)
        {
            if ((oldLoan?.Status ?? loan.Status) == Loan.STATUS_COMPLETE)
                throw new BusinessLogicException("Loan is already completed!");

            if (loan.LoanTypeID == null)
                throw new BusinessLogicException("Loan type is required.");

            if (loan.DedEffectiveDateFrom < PayrollTools.SqlServerMinimumDate)
                throw new BusinessLogicException("Date cannot be earlier than January 1, 1753.");

            if (loan.TotalLoanAmount <= 0)
                throw new BusinessLogicException("Total loan amount cannot be less than or equal to 0.");

            if (loan.DeductionAmount <= 0)
                throw new BusinessLogicException("Deduction amount cannot be less than or equal to 0.");

            if (string.IsNullOrWhiteSpace(loan.DeductionSchedule))
                throw new BusinessLogicException("Deduction schedule is required.");

            if (!_loanRepository.GetStatusList().Any(x => x.ToTrimmedLowerCase() == loan.Status.ToTrimmedLowerCase()))
                throw new BusinessLogicException("Status is required.");
        }

        private async Task ValidationForBenchmark(Loan loan)
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
                if (loan.Status == Loan.STATUS_IN_PROGRESS)
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

        private bool CheckIfStartDateNeedsToBeValidated(List<Loan> oldLoans, Loan loan)
        {
            // either a new loan
            if (loan.IsNewEntity) return true;

            // or an existing loan where its Start Date is to be updated
            var oldLoan = oldLoans.Where(o => o.RowID == loan.RowID).FirstOrDefault();

            if (loan.DedEffectiveDateFrom.ToMinimumHourValue() != oldLoan.DedEffectiveDateFrom.ToMinimumHourValue())
                return true;

            return false;
        }

        private async Task AddBasicMonthlySalaryData(Loan loan)
        {
            if (loan.IsNewEntity)
            {
                var currentEmployee = await _employeeRepository
                    .GetByIdAsync(loan.EmployeeID.Value);

                var currentSalary = await _employeeRepository
                    .GetCurrentSalaryAsync(loan.EmployeeID.Value, loan.DedEffectiveDateFrom);

                loan.BasicMonthlySalary = PayrollTools.GetEmployeeMonthlyRate(currentEmployee, currentSalary);
            }
        }

        #endregion Private Methods

        #region List of entities

        public async Task<ICollection<Loan>> GetCurrentPayrollLoansAsync(int organizationId,
            PayPeriod payPeriod,
            IReadOnlyCollection<Paystub> paystubs)
        {
            return await _loanRepository.GetCurrentPayrollLoansAsync(
                organizationId: organizationId,
                payPeriod: payPeriod,
                paystubs: paystubs);
        }

        #endregion List of entities
    }
}
