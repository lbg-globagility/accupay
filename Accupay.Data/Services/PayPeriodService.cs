using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class PayPeriodService
    {
        private readonly PayrollContext _context;
        private readonly PayPeriodRepository _repository;
        private readonly SystemOwnerService _systemOwnerService;

        public PayPeriodService(PayrollContext context,
                                PayPeriodRepository repository,
                                SystemOwnerService systemOwnerService)
        {
            _context = context;
            _repository = repository;
            _systemOwnerService = systemOwnerService;
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
    }
}