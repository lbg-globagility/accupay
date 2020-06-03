using AccuPay.Data.Repositories;
using AccuPay.Data.Services;
using AccuPay.Web.Core.Auth;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.TimeEntries
{
    public class TimeEntryService
    {
        private readonly PayPeriodRepository _payPeriodRepository;
        private readonly TimeEntryGenerator _generator;
        private readonly ICurrentUser _currentUser;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public TimeEntryService(PayPeriodRepository payPeriodRepository, TimeEntryGenerator generator, ICurrentUser currentUser, IServiceScopeFactory serviceScopeFactory)
        {
            _payPeriodRepository = payPeriodRepository;
            _generator = generator;
            _currentUser = currentUser;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Generate(int payPeriodId)
        {
            var payPeriod = await _payPeriodRepository.GetByIdAsync(payPeriodId);
            //using (var scope = _serviceScopeFactory.CreateScope())
            //{
            //    var generator = scope.ServiceProvider.GetRequiredService<TimeEntryGenerator>();

            //    await Task.Run(() => generator.Start(_currentUser.OrganizationId, payPeriod.PayFromDate, payPeriod.PayToDate));
            //}

            _generator.Start(_currentUser.OrganizationId, payPeriod.PayFromDate, payPeriod.PayToDate);
        }
    }
}
