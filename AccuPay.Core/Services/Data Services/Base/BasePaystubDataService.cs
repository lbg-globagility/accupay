using AccuPay.Core.Entities;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Repositories;
using System.Threading.Tasks;

namespace AccuPay.Core.Services
{
    public abstract class BasePaystubDataService
    {
        protected readonly PayPeriodRepository _payPeriodRepository;

        public BasePaystubDataService(PayPeriodRepository payPeriodRepository)
        {
            _payPeriodRepository = payPeriodRepository;
        }

        protected async Task ValidateIfPayPeriodIsOpenAsync(int organizationId, int? payPeriodId)
        {
            var currentOpenPayPeriod = await _payPeriodRepository.GetCurrentOpenAsync(organizationId);

            if (currentOpenPayPeriod == null || currentOpenPayPeriod?.RowID != payPeriodId)
                throw new BusinessLogicException("Only open pay periods can be modified.");
        }

        protected void ValidateIfPayPeriodIsOpenAsync(PayPeriod payPeriod)
        {
            if (payPeriod == null || payPeriod.IsOpen == false)
                throw new BusinessLogicException("Only open pay periods can be modified.");
        }
    }
}
