using AccuPay.Data.Repositories;
using System.Collections.Generic;
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

        public async Task<ICollection<PaystubData>> GetAll(int payPeriodId)
        {
            var paystubs = await _paystubRepository.GetAll(payPeriodId);

            var payPeriod = await _payPeriodRepository.GetByIdAsync(payPeriodId);

            var employeeIds = paystubs.Select(x => x.EmployeeID.Value).ToArray();

            var salaries = await _salaryRepository.GetByMultipleEmployeeAsync(employeeIds, payPeriod.PayFromDate);

            var paystubsWithSalary = paystubs.Select(x =>
                new PaystubData(x, salaries.FirstOrDefault(s => s.EmployeeID == x.EmployeeID))).ToList();

            return paystubsWithSalary;
        }
    }
}