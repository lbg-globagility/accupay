using AccuPay.Data.Entities;
using AccuPay.Data.Enums;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class PayPeriodDataService
    {
        private readonly PayrollContext _context;
        private readonly PolicyHelper _policy;
        private readonly PayPeriodRepository _repository;
        private readonly SystemOwnerService _systemOwnerService;

        public PayPeriodDataService(
            PayrollContext context,
            PayPeriodRepository repository,
            SystemOwnerService systemOwnerService,
            PolicyHelper policy)
        {
            _context = context;
            _repository = repository;
            _systemOwnerService = systemOwnerService;
            _policy = policy;
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

            var payPeriod = await _repository.GetByIdAsync(payPeriodId.Value);

            if (payPeriod == null)
            {
                return FunctionResult.Failed("Pay period does not exists. Please refresh the form.");
            }

            // TODO: this should be queried from _repository
            // remove _context from this class
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

        public async Task<PayPeriod> StartStatusAsync(int organizationId, int month, int year, bool isFirstHalf, int userId)
        {
            var payPeriod = await _repository.GetAsync(
                organizationId,
                month: month,
                year: year,
                isFirstHalf: isFirstHalf);

            if (payPeriod == null)
            {
                payPeriod = PayPeriod.NewPayPeriod(organizationId, month, year, isFirstHalf, _policy);
                await _repository.CreateAsync(payPeriod);
            }

            await UpdateStatusAsync(payPeriod, userId, PayPeriodStatus.Open);

            return payPeriod;
        }

        public async Task CloseStatusAsync(int payPeriodId, int userId)
        {
            await UpdateStatusAsync(payPeriodId, userId, PayPeriodStatus.Closed);
        }

        public async Task UpdateStatusAsync(int payPeriodId, int userId, PayPeriodStatus status)
        {
            var payPeriod = await _repository.GetByIdAsync(payPeriodId);

            if (payPeriod == null)
                throw new BusinessLogicException("Pay Period does not exists");

            payPeriod.Status = status;
            payPeriod.LastUpdBy = userId;

            await _repository.UpdateAsync(payPeriod);
        }

        public async Task UpdateStatusAsync(PayPeriod payPeriod, int userId, PayPeriodStatus status)
        {
            if (payPeriod?.RowID == null)
                throw new BusinessLogicException("Pay Period does not exists");

            if ((await _repository.GetByIdAsync(payPeriod.RowID.Value)) == null)
                throw new BusinessLogicException("Pay Period does not exists");

            payPeriod.Status = status;
            payPeriod.LastUpdBy = userId;

            await _repository.UpdateAsync(payPeriod);
        }
    }
}