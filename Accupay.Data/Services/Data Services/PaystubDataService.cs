using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class PaystubDataService
    {
        private readonly PaystubRepository _paystubRepository;
        private readonly SalaryRepository _salaryRepository;
        private readonly PayPeriodRepository _payPeriodRepository;

        public PaystubDataService(PaystubRepository paystubRepository, SalaryRepository salaryRepository, PayPeriodRepository payPeriodRepository)
        {
            _paystubRepository = paystubRepository;
            _salaryRepository = salaryRepository;
            _payPeriodRepository = payPeriodRepository;
        }

        public async Task<PaginatedListResult<PaystubData>> GetPaginatedListAsync(
            PageOptions options,
            int payPeriodId,
            string searchTerm = "")
        {
            var paginatedList = await _paystubRepository.GetPaginatedListAsync(options, payPeriodId, searchTerm);

            var payPeriod = await _payPeriodRepository.GetByIdAsync(payPeriodId);

            var employeeIds = paginatedList.List.Select(x => x.EmployeeID.Value).ToArray();

            var salaries = await _salaryRepository.GetByMultipleEmployeeAsync(employeeIds, payPeriod.PayFromDate);

            var paystubsWithSalary = paginatedList.List.Select(x =>
                new PaystubData(x, salaries.FirstOrDefault(s => s.EmployeeID == x.EmployeeID))).ToList();

            return new PaginatedListResult<PaystubData>(paystubsWithSalary, paginatedList.TotalCount);
        }
    }
}