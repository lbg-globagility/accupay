using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Payroll
{
    public class PaystubService
    {
        private readonly PaystubRepository _repository;

        public PaystubService(PaystubRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedList<PaystubDto>> PaginatedList(
            PageOptions options,
            int payperiodId,
            string searchTerm = "")
        {
            var paginatedList = await _repository.GetPaginatedListAsync(options, payperiodId, searchTerm);
            var dtos = paginatedList.List.Select(t => PaystubDto.Convert(t.Employee, t)).ToList();

            return new PaginatedList<PaystubDto>(dtos, paginatedList.TotalCount, ++options.PageIndex, options.PageSize);
        }
    }
}
