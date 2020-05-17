using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class PayPeriodService
    {
        private readonly PayrollContext _context;
        private readonly SystemOwnerService _systemOwnerService;
        private readonly PayPeriodRepository _payPeriodRepository;

        public PayPeriodService(PayrollContext context,
                                SystemOwnerService systemOwnerService,
                                PayPeriodRepository payPeriodRepository)
        {
            _context = context;
            _systemOwnerService = systemOwnerService;
            _payPeriodRepository = payPeriodRepository;
        }

        public async Task<IPayPeriod> GetCurrentlyWorkedOnPayPeriodByCurrentYearAsync(int organizationId,
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

            var payPeriod = await _context.PayPeriods.
                            FirstOrDefaultAsync(p => p.RowID == payPeriodId);

            if (payPeriod == null)
            {
                return FunctionResult.Failed("Pay period does not exists. Please refresh the form.");
            }

            var otherProcessingPayPeriod = await _context.Paystubs.
                                                        Include(p => p.PayPeriod).
                                                        Where(p => p.PayPeriod.RowID != payPeriodId).
                                                        Where(p => p.PayPeriod.IsClosed == false).
                                                        Where(p => p.PayPeriod.OrganizationID == organizationId).
                                                        FirstOrDefaultAsync();

            if (payPeriod.IsClosed)
            {
                return FunctionResult.Failed("The pay period you selected is already closed. Please reopen so you can alter the data for that pay period. If there are \"Processing\" pay periods, make sure to close them first.");
            }
            else if (!payPeriod.IsClosed && otherProcessingPayPeriod != null)
            {
                return FunctionResult.Failed("There is currently a pay period with \"PROCESSING\" status. Please finish that pay period first then close it to process other open pay periods.");
            }

            return FunctionResult.Success();
        }
    }
}