using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services;
using AccuPay.Web.Loans;
using System.Collections;
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

        public async Task<PaginatedList<PaystubDto>> PaginatedList(
            PageOptions options,
            int payPeriodId,
            string searchTerm = "")
        {
            var paginatedList = await _service.GetPaginatedListAsync(options, payPeriodId, searchTerm);
            var dtos = paginatedList.List.Select(t => PaystubDto.Convert(t)).ToList();

            return new PaginatedList<PaystubDto>(dtos, paginatedList.TotalCount, ++options.PageIndex, options.PageSize);
        }

        public async Task<List<LoanTransactionDto>> GetLoanTransactions(int paystubId)
        {
            var loanTransactions = await repository.GetLoanTransanctionsAsync(paystubId);

            return loanTransactions.Select(x => LoanTransactionDto.Convert(x)).ToList();
        }
    }
}
