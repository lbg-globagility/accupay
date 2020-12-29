using AccuPay.Data.Entities;
using AccuPay.Data.Enums;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class PayPeriodDataService
    {
        private readonly IPolicyHelper _policy;
        private readonly PayPeriodRepository _payPeriodRepository;
        private readonly PaystubRepository _paystubRepository;
        private readonly TimeEntryRepository _timeEntryRepository;
        private readonly SystemOwnerService _systemOwnerService;

        public PayPeriodDataService(
            PayPeriodRepository payPeriodRepository,
            PaystubRepository paystubRepository,
            TimeEntryRepository timeEntryRepository,
            SystemOwnerService systemOwnerService,
            IPolicyHelper policy)
        {
            _payPeriodRepository = payPeriodRepository;
            _paystubRepository = paystubRepository;
            _timeEntryRepository = timeEntryRepository;
            _systemOwnerService = systemOwnerService;
            _policy = policy;
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

        public async Task CancelAsync(int payPeriodId, int userId)
        {
            await UpdateStatusAsync(payPeriodId, userId, PayPeriodStatus.Pending);

            await _paystubRepository.DeleteByPeriodAsync(payPeriodId, userId);

            await _timeEntryRepository.DeleteByPayPeriodAsync(payPeriodId);
        }

        public async Task UpdateStatusAsync(int payPeriodId, int userId, PayPeriodStatus status)
        {
            var payPeriod = await _payPeriodRepository.GetByIdAsync(payPeriodId);

            await UpdateStatusAsync(payPeriod, userId, status);
        }

        public async Task UpdateStatusAsync(PayPeriod payPeriod, int userId, PayPeriodStatus status)
        {
            if (payPeriod?.RowID == null || payPeriod?.OrganizationID == null)
                throw new BusinessLogicException("Pay Period does not exists.");

            if ((await _payPeriodRepository.GetByIdAsync(payPeriod.RowID.Value)) == null)
                throw new BusinessLogicException("Pay Period does not exists.");

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
            payPeriod.LastUpdBy = userId;

            await _payPeriodRepository.UpdateAsync(payPeriod);
        }
    }
}
