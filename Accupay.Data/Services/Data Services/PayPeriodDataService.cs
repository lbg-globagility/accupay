using AccuPay.Data.Entities;
using AccuPay.Data.Enums;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class PayPeriodDataService
    {
        private const string UserActivityName = "Pay Period";

        private readonly IPolicyHelper _policy;
        private readonly PaystubDataHelper _paystubDataHelper;
        private readonly TimeEntryDataHelper _timeEntryDataHelper;
        private readonly UserActivityRepository _userActivityRepository;
        private readonly PayPeriodRepository _payPeriodRepository;
        private readonly PaystubRepository _paystubRepository;
        private readonly TimeEntryRepository _timeEntryRepository;
        private readonly SystemOwnerService _systemOwnerService;

        public PayPeriodDataService(
            PayPeriodRepository payPeriodRepository,
            PaystubRepository paystubRepository,
            TimeEntryRepository timeEntryRepository,
            SystemOwnerService systemOwnerService,
            IPolicyHelper policy,
            PaystubDataHelper paystubDataHelper,
            TimeEntryDataHelper timeEntryDataHelper,
            UserActivityRepository userActivityRepository)
        {
            _payPeriodRepository = payPeriodRepository;
            _paystubRepository = paystubRepository;
            _timeEntryRepository = timeEntryRepository;
            _systemOwnerService = systemOwnerService;
            _policy = policy;
            _paystubDataHelper = paystubDataHelper;
            _timeEntryDataHelper = timeEntryDataHelper;
            _userActivityRepository = userActivityRepository;
        }

        public static string HasCurrentlyOpenErrorMessage(PayPeriod payPeriod)
        {
            var payPeriodString = $"{payPeriod.PayFromDate.ToShortDateString()} - {payPeriod.PayToDate.ToShortDateString()}";
            return $"There is currently an \"Open\" pay period. Please finish the pay period {payPeriodString} first then close it to process new pay periods.";
        }

        public async Task<FunctionResult> ValidatePayPeriodActionAsync(int? payPeriodId)
        {
            if (_systemOwnerService.GetCurrentSystemOwner() == SystemOwnerService.Benchmark)
            {
                // Add temporarily. Consult maam mely first as she is still testing the system with multiple pay periods
                return FunctionResult.Success();
            }

            if (payPeriodId == null)
            {
                return FunctionResult.Failed("Pay period does not exists. Please refresh the form.");
            }

            var payPeriod = await _payPeriodRepository.GetByIdAsync(payPeriodId.Value);

            if (payPeriod == null)
            {
                return FunctionResult.Failed("Pay period does not exists. Please refresh the form.");
            }

            if (payPeriod.Status != PayPeriodStatus.Open)
            {
                return FunctionResult.Failed("Only open pay periods can be modified.");
            }

            return FunctionResult.Success();
        }

        public async Task<PayPeriod> CreateAsync(int organizationId, int month, int year, bool isFirstHalf, int currentUserId)
        {
            var payPeriod = PayPeriod.NewPayPeriod(
                organizationId: organizationId,
                payrollMonth: month,
                payrollYear: year,
                isFirstHalf: isFirstHalf,
                policy: _policy,
                currentUserId: currentUserId);
            await _payPeriodRepository.CreateAsync(payPeriod);

            return payPeriod;
        }

        public async Task<PayPeriod> StartStatusAsync(int organizationId, int month, int year, bool isFirstHalf, int currentUserId)
        {
            var payPeriod = await _payPeriodRepository.GetAsync(
                organizationId,
                month: month,
                year: year,
                isFirstHalf: isFirstHalf);

            if (payPeriod == null)
            {
                payPeriod = await CreateAsync(
                    organizationId: organizationId,
                    month: month,
                    year: year,
                    isFirstHalf: isFirstHalf,
                    currentUserId: currentUserId);
            }

            await UpdateStatusAsync(payPeriod, currentUserId, PayPeriodStatus.Open);

            return payPeriod;
        }

        public async Task CloseAsync(int payPeriodId, int userId)
        {
            await UpdateStatusAsync(payPeriodId, userId, PayPeriodStatus.Closed);
        }

        public async Task ReopenAsync(int payPeriodId, int userId)
        {
            await UpdateStatusAsync(payPeriodId, userId, PayPeriodStatus.Open);
        }

        public async Task CancelAsync(int payPeriodId, int currentlyLoggedInUserId)
        {
            var payPeriod = await UpdateStatusAsync(payPeriodId, currentlyLoggedInUserId, PayPeriodStatus.Pending);

            var deletedPaystubs = await _paystubRepository.DeleteByPeriodAsync(payPeriodId, currentlyLoggedInUserId);

            var deletedTimeEntries = await _timeEntryRepository.DeleteByPayPeriodAsync(payPeriodId);

            // save user activities for time entries and paystubs

            // TODO: maybe record delete for adjustments
            await _paystubDataHelper.RecordDelete(currentlyLoggedInUserId, deletedPaystubs, payPeriod);

            await _timeEntryDataHelper.RecordDelete(currentlyLoggedInUserId, deletedTimeEntries.timeEntries);
        }

        public async Task<PayPeriod> UpdateStatusAsync(int payPeriodId, int userId, PayPeriodStatus status)
        {
            var payPeriod = await _payPeriodRepository.GetByIdAsync(payPeriodId);

            await UpdateStatusAsync(payPeriod, userId, status);

            return payPeriod;
        }

        public async Task UpdateStatusAsync(PayPeriod payPeriod, int currentlyLoggedInUserId, PayPeriodStatus status)
        {
            if (payPeriod?.RowID == null || payPeriod?.OrganizationID == null)
                throw new BusinessLogicException("Pay Period does not exists.");

            if ((await _payPeriodRepository.GetByIdAsync(payPeriod.RowID.Value)) == null)
                throw new BusinessLogicException("Pay Period does not exists.");

            if (payPeriod.Status == status) return;

            if (status == PayPeriodStatus.Open)
            {
                var currentOpenPayPeriod = await _payPeriodRepository.GetCurrentOpenAsync(payPeriod.OrganizationID.Value);
                if (currentOpenPayPeriod != null)
                {
                    throw new BusinessLogicException(HasCurrentlyOpenErrorMessage(currentOpenPayPeriod));
                }

                var hasClosedPayPeriodsAfterDate = await _payPeriodRepository.HasClosedPayPeriodAfterDateAsync(
                    payPeriod.OrganizationID.Value,
                    payPeriod.PayToDate.AddDays(1));

                if (hasClosedPayPeriodsAfterDate)
                {
                    throw new BusinessLogicException("Cannot open a pay period if there are closed pay periods ahead of the selected period.");
                }
            }

            payPeriod.Status = status;
            payPeriod.LastUpdBy = currentlyLoggedInUserId;

            await _payPeriodRepository.UpdateAsync(payPeriod);

            await RecordUserActivity(payPeriod, currentlyLoggedInUserId, status);
        }

        #region Private Methods

        private async Task RecordUserActivity(PayPeriod payPeriod, int currentlyLoggedInUserId, PayPeriodStatus status)
        {
            var userActivityAction = string.Empty;
            switch (status)
            {
                case PayPeriodStatus.Pending:
                    userActivityAction = "Cancelled";
                    break;

                case PayPeriodStatus.Open:
                    userActivityAction = "Opened";
                    break;

                case PayPeriodStatus.Closed:
                    userActivityAction = "Closed";
                    break;

                default:
                    break;
            }

            var activityItem = new List<UserActivityItem>()
            {
                new UserActivityItem()
                {
                    EntityId = payPeriod.RowID.Value,
                    Description = $"{userActivityAction} the payroll '{payPeriod.PayFromDate.ToShortDateString()}' to '{payPeriod.PayToDate.ToShortDateString()}'.",
                    ChangedEmployeeId = null
                }
            };

            await _userActivityRepository.CreateRecordAsync(
                currentlyLoggedInUserId,
                UserActivityName,
                payPeriod.OrganizationID.Value,
                UserActivityRepository.RecordTypeEdit,
                activityItem);
        }

        #endregion Private Methods
    }
}
