using AccuPay.Data.Enums;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Repositories;
using AccuPay.Data.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class BaseDataService
    {
        private const string ClosedPayPeriodErrorMessage = "Data cannot be modified since it is within a \"Closed\" pay period.";

        protected readonly PayPeriodRepository _payPeriodRepository;
        protected readonly PolicyHelper _policy;

        public BaseDataService(PayPeriodRepository payPeriodRepository, PolicyHelper policy)
        {
            _payPeriodRepository = payPeriodRepository;
            _policy = policy;
        }

        public async Task CheckIfDataIsWithinClosedPayroll(DateTime date, int organizationId)
        {
            var currentPayPeriod = await _payPeriodRepository.GetAsync(organizationId, date);

            if (currentPayPeriod == null) return;

            bool isClosed;

            if (_policy.PayrollClosingType == PayrollClosingType.IsClosed)
            {
                isClosed = currentPayPeriod.IsClosed;
            }
            else
            {
                isClosed = currentPayPeriod.Status == PayPeriodStatus.Closed;
            }

            if (isClosed)
                throw new BusinessLogicException(ClosedPayPeriodErrorMessage);
        }

        public async Task CheckIfDataIsWithinClosedPayroll(IEnumerable<DateTime> dates, int organizationId)
        {
            if (!dates.Any()) return;

            var earliestDate = dates.Min();
            var latestDate = dates.Max();
            var dateRange = new TimePeriod(earliestDate, latestDate);

            var closedPayPeriods = (await _payPeriodRepository.GetClosedPayPeriodsAsync(organizationId, dateRange)).ToList();

            foreach (var date in dates)
            {
                if (closedPayPeriods.Any(x => x.IsBetween(date)))
                    throw new BusinessLogicException("Data cannot be modified since it is within a \"Closed\" pay period.");
            }
        }
    }
}