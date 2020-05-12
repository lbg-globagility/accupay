using AccuPay.Data.Repositories;
using AccuPay.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class PayPeriodService
    {
        private readonly SystemOwnerService _systemOwnerService;
        private readonly PayPeriodRepository _payPeriodRepository;

        public PayPeriodService(SystemOwnerService systemOwnerService,
                                PayPeriodRepository payPeriodRepository)
        {
            _systemOwnerService = systemOwnerService;
            _payPeriodRepository = payPeriodRepository;
        }

        public async Task<IPayPeriod> GetCurrentlyWorkedOnPayPeriodByCurrentYear(int organizationId,
                                                            IEnumerable<IPayPeriod> payperiods = null)
        {
            // replace this with a policy
            // fourlinq can use this feature also
            // for clients that has the same attendance and payroll period
            var isBenchmarkOwner = _systemOwnerService.GetCurrentSystemOwner() ==
                                            SystemOwnerService.Benchmark;

            var currentDay = DateTime.Today.ToMinimumHourValue();

            if (payperiods == null || payperiods.Count() == 0)
            {
                payperiods = await _payPeriodRepository.GetAllSemiMonthlyAsync(organizationId);
            }

            if (isBenchmarkOwner)
            {
                return payperiods.
                        Where(p => currentDay >= p.PayFromDate && currentDay <= p.PayToDate).
                        LastOrDefault();
            }
            else
            {
                return payperiods.
                        Where(p => p.PayToDate < currentDay).
                        LastOrDefault();
            }
        }
    }
}