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
        public readonly PayPeriodRepository _payPeriodRepository;

        public BaseDataService(PayPeriodRepository payPeriodRepository)
        {
            _payPeriodRepository = payPeriodRepository;
        }

        public async Task CheckIfDataIsWithinClosedPayroll(DateTime date, int organizationId)
        {
            var currentPayPeriod = await _payPeriodRepository.GetAsync(organizationId, date);

            if (currentPayPeriod?.Status == PayPeriodStatus.Closed)
                throw new BusinessLogicException("Data cannot be updated since it is within a \"Closed\" pay period.");
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
                    throw new BusinessLogicException("Data cannot be updated since it is within a \"Closed\" pay period.");
            }
        }
    }
}