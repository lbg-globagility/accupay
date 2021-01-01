using AccuPay.Core.Repositories;
using AccuPay.Core.Services;
using AccuPay.Web.Loans;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Payroll
{
    public class PaystubService
    {
        private readonly PaystubDataService _service;
        private readonly PaystubRepository repository;

        public PaystubService(PaystubDataService service, PaystubRepository repository)
        {
            _service = service;
            this.repository = repository;
        }

        public async Task<ICollection<PaystubDto>> GetAll(int payPeriodId)
        {
            var paystubs = await _service.GetAllAsync(payPeriodId);
            var dtos = paystubs.Select(t => PaystubDto.Convert(t)).ToList();

            return dtos;
        }

        public async Task<List<AdjustmentDto>> GetAdjustments(int paystubId)
        {
            var adjustments = await repository.GetAdjustmentsAsync(paystubId);

            return adjustments.Select(x => AdjustmentDto.Convert(x)).ToList();
        }

        public async Task<List<LoanTransactionDto>> GetLoanTransactions(int paystubId)
        {
            var loanTransactions = await repository.GetLoanTransactionsAsync(paystubId);

            return loanTransactions.Select(x => LoanTransactionDto.Convert(x)).ToList();
        }
    }
}
