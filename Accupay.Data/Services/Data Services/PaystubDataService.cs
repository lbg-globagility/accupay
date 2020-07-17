using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AccuPay.Data.Repositories.PaystubRepository;

namespace AccuPay.Data.Services
{
    public class PaystubDataService
    {
        private readonly PaystubRepository _paystubRepository;
        private readonly SalaryRepository _salaryRepository;
        private readonly PayPeriodRepository _payPeriodRepository;

        public PaystubDataService(
            PaystubRepository paystubRepository,
            SalaryRepository salaryRepository,
            PayPeriodRepository payPeriodRepository)
        {
            _paystubRepository = paystubRepository;
            _salaryRepository = salaryRepository;
            _payPeriodRepository = payPeriodRepository;
        }

        public async Task<ICollection<PaystubData>> GetAll(int payPeriodId)
        {
            var paystubs = await _paystubRepository.GetByPayPeriodFullPaystubAsync(payPeriodId);

            var payPeriod = await _payPeriodRepository.GetByIdAsync(payPeriodId);

            var employeeIds = paystubs.Select(x => x.EmployeeID.Value).ToArray();

            var salaries = await _salaryRepository.GetByMultipleEmployeeAsync(employeeIds, payPeriod.PayFromDate);

            var paystubsWithSalary = paystubs.Select(x =>
                new PaystubData(x, salaries.FirstOrDefault(s => s.EmployeeID == x.EmployeeID))).ToList();

            return paystubsWithSalary;
        }

        public async Task DeleteAsync(EmployeeCompositeKey key, int userId, int organizationId)
        {
            var paystub = await _paystubRepository.GetByCompositeKeyAsync(key);

            if (paystub == null)
                throw new BusinessLogicException("Paystub does not exists.");

            var payPeriodId = (await _paystubRepository.GetWithPayPeriod(paystub.RowID.Value))?.PayPeriod?.RowID;

            var currentOpenPayPeriod = await _payPeriodRepository.GetCurrentOpenAsync(organizationId);

            if (currentOpenPayPeriod == null || currentOpenPayPeriod?.RowID != payPeriodId)
                throw new BusinessLogicException("Only open pay periods can be modified.");

            await _paystubRepository.DeleteAsync(paystub.RowID.Value, userId);
        }

        public async Task DeleteByPeriodAsync(int payPeriodId, int userId, int organizationId)
        {
            // TODO: extract this to be reusable
            var currentOpenPayPeriod = await _payPeriodRepository.GetCurrentOpenAsync(organizationId);

            if (currentOpenPayPeriod == null || currentOpenPayPeriod?.RowID != payPeriodId)
                throw new BusinessLogicException("Only open pay periods can be modified.");

            await _paystubRepository.DeleteByPeriodAsync(payPeriodId, userId);
        }

        public async Task UpdateManyThirteenthMonthPaysAsync(IEnumerable<ThirteenthMonthPay> thirteenthMonthPays)
        {
            var paystubIds = thirteenthMonthPays
                .Where(x => x.RowID.HasValue)
                .Select(x => x.PaystubID.Value)
                .Distinct()
                .ToArray();

            var payPeriods = (await _paystubRepository.GetWithPayPeriod(paystubIds))?.Select(x => x.PayPeriod);

            // TODO:
            // get payperiods through paystubIds
            // validate payperiod

            await _paystubRepository.UpdateManyThirteenthMonthPaysAsync(thirteenthMonthPays);
        }
    }
}