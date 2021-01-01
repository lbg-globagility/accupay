using AccuPay.Core.Entities;
using AccuPay.Core.Enums;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Repositories;
using AccuPay.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Core.Services
{
    public abstract class BaseDataService
    {
        private const string ClosedPayPeriodErrorMessage = "Data cannot be modified since it is within a \"Closed\" pay period.";

        protected readonly PayPeriodRepository _payPeriodRepository;
        protected readonly IPolicyHelper _policy;

        public BaseDataService(PayPeriodRepository payPeriodRepository, IPolicyHelper policy)
        {
            _payPeriodRepository = payPeriodRepository;
            _policy = policy;
        }

        public async Task<bool> CheckIfDataIsWithinClosedPayPeriod(DateTime date, int organizationId, bool throwException = true)
        {
            var currentPayPeriod = await _payPeriodRepository.GetAsync(organizationId, date);

            if (currentPayPeriod == null) return false;

            if (currentPayPeriod.Status == PayPeriodStatus.Closed)
            {
                if (throwException)
                {
                    throw new BusinessLogicException(ClosedPayPeriodErrorMessage);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CheckIfDataIsWithinClosedPayPeriod(IEnumerable<DateTime> dates, int organizationId, bool throwException = true)
        {
            if (!dates.Any()) return false;

            var earliestDate = dates.Min();
            var latestDate = dates.Max();
            var dateRange = new TimePeriod(earliestDate, latestDate);

            var closedPayPeriods = (await _payPeriodRepository.GetClosedPayPeriodsAsync(organizationId, dateRange)).ToList();

            foreach (var date in dates)
            {
                if (closedPayPeriods.Any(x => x.IsBetween(date)))
                {
                    if (throwException)
                    {
                        throw new BusinessLogicException(ClosedPayPeriodErrorMessage);
                    }
                    return true;
                }
            }

            return false;
        }

        public bool CheckIfDataIsWithinClosedPayPeriod(IEnumerable<PayPeriod> payPeriods, bool throwException = true)
        {
            var hasClosedPayPeriod = payPeriods.Where(x => x.Status == PayPeriodStatus.Closed).Any();

            if (hasClosedPayPeriod)
            {
                if (throwException)
                {
                    throw new BusinessLogicException(ClosedPayPeriodErrorMessage);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
