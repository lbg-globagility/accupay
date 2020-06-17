using AccuPay.Data.Helpers;
using AccuPay.Data.Services;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Payroll
{
    public class PaystubService
    {
        private readonly PaystubDataService _service;

        public PaystubService(PaystubDataService service)
        {
            _service = service;
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
    }
}
