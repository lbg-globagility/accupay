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
        private readonly PolicyHelper _policy;
        private readonly PayPeriodRepository _payPeriodRepository;
        private readonly PaystubRepository _paystubRepository;
        private readonly SystemOwnerService _systemOwnerService;

        public PayPeriodDataService(
            PayPeriodRepository payPeriodRepository,
            PaystubRepository paystubRepository,
            SystemOwnerService systemOwnerService,
            PolicyHelper policy)
        {
            _payPeriodRepository = payPeriodRepository;
            _paystubRepository = paystubRepository;
            _systemOwnerService = systemOwnerService;
            _policy = policy;
        }

        public static string HasCurrentlyOpenErrorMessage(PayPeriod payPeriod)
        {
            var payPeriodString = $"{payPeriod.PayFromDate.ToShortDateString()} - {payPeriod.PayToDate.ToShortDateString()}";
            return $"There is currently an \"Open\" pay period. Please finish the pay period {payPeriodString} first then close it to process new pay periods.";
        }

        public async Task<FunctionResult> ValidatePayPeriodActionAsync(int? payPeriodId, int organizationId)
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

        public async Task<PayPeriod> StartStatusAsync(int organizationId, int month, int year, bool isFirstHalf, int userId)
        {
            var payPeriod = await _payPeriodRepository.GetAsync(
                organizationId,
                month: month,
                year: year,
                isFirstHalf: isFirstHalf);

            if (payPeriod == null)
            {
                payPeriod = PayPeriod.NewPayPeriod(organizationId, month, year, isFirstHalf, _policy);
                await _payPeriodRepository.CreateAsync(payPeriod);
            }

            await UpdateStatusAsync(payPeriod, userId, PayPeriodStatus.Open);

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
        }

        public async Task UpdateStatusAsync(int payPeriodId, int userId, PayPeriodStatus status)
        {
            var payPeriod = await _payPeriodRepository.GetByIdAsync(payPeriodId);

            await UpdateStatusAsync(payPeriod, userId, status);
        }

        public async Task UpdateStatusAsync(PayPeriod payPeriod, int userId, PayPeriodStatus status)
        {
            if (payPeriod?.RowID == null || payPeriod?.OrganizationID == null)
                throw new BusinessLogicException("Pay Period does not exists");

            if ((await _payPeriodRepository.GetByIdAsync(payPeriod.RowID.Value)) == null)
                throw new BusinessLogicException("Pay Period does not exists");

            if (status == PayPeriodStatus.Open)
            {
                var currentOpenPayPeriod = await _payPeriodRepository.GetCurrentOpenAsync(payPeriod.OrganizationID.Value);
                if (currentOpenPayPeriod != null)
                    throw new BusinessLogicException(HasCurrentlyOpenErrorMessage(currentOpenPayPeriod));
            }

            payPeriod.Status = status;
            payPeriod.LastUpdBy = userId;

            await _payPeriodRepository.UpdateAsync(payPeriod);
        }
    }
}