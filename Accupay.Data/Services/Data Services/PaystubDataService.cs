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
        private readonly PaystubEmailRepository _paystubEmailRepository;
        private readonly PaystubEmailHistoryRepository _paystubEmailHistoryRepository;
        private readonly SalaryRepository _salaryRepository;
        private readonly PayPeriodRepository _payPeriodRepository;

        public PaystubDataService(
            PaystubRepository paystubRepository,
            PaystubEmailRepository paystubEmailRepository,
            PaystubEmailHistoryRepository paystubEmailHistoryRepository,
            SalaryRepository salaryRepository,
            PayPeriodRepository payPeriodRepository)
        {
            _paystubRepository = paystubRepository;
            _paystubEmailRepository = paystubEmailRepository;
            _paystubEmailHistoryRepository = paystubEmailHistoryRepository;
            _salaryRepository = salaryRepository;
            _payPeriodRepository = payPeriodRepository;
        }

        public async Task<ICollection<PaystubData>> GetAll(int payPeriodId)
        {
            var paystubs = await _paystubRepository.GetByPayPeriodFullPaystubAsync(payPeriodId);

            var payPeriod = await _payPeriodRepository.GetByIdAsync(payPeriodId);

            var employeeIds = paystubs.Select(x => x.EmployeeID.Value).ToArray();

            var salaries = await _salaryRepository.GetByMultipleEmployeeAsync(employeeIds, payPeriod.PayFromDate);

            var paystubsWithSalary = paystubs
                .Select(paystub => new PaystubData(
                    paystub,
                    salaries.FirstOrDefault(s => s.EmployeeID == paystub.EmployeeID)
                )).ToList();

            return paystubsWithSalary;
        }

        public async Task DeleteAsync(EmployeeCompositeKey key, int userId, int organizationId)
        {
            var paystub = await _paystubRepository.GetByCompositeKeyAsync(key);
            await DeleteAsync(paystub, userId: userId, organizationId: organizationId);
        }

        public async Task DeleteAsync(Paystub paystub, int userId, int organizationId)
        {
            if (paystub == null)
                throw new BusinessLogicException("Paystub does not exists.");

            var payPeriodId = (await _paystubRepository.GetWithPayPeriod(paystub.RowID.Value))?.PayPeriod?.RowID;

            await ValidateIfPayPeriodIsOpenAsync(
                organizationId: organizationId,
                payPeriodId: payPeriodId);

            await _paystubRepository.DeleteAsync(
                id: paystub.RowID.Value,
                userId: userId);
        }

        public async Task DeleteByPeriodAsync(int payPeriodId, int userId, int organizationId)
        {
            await ValidateIfPayPeriodIsOpenAsync(
                organizationId: organizationId,
                payPeriodId: payPeriodId);

            await _paystubRepository.DeleteByPeriodAsync(
                payPeriodId: payPeriodId,
                userId: userId);
        }

        public async Task DeletePaystubEmailsByPeriodAsync(int payPeriodId, int organizationId)
        {
            await ValidateIfPayPeriodIsOpenAsync(
                organizationId: organizationId,
                payPeriodId: payPeriodId);

            await _paystubEmailRepository.DeleteByPayPeriodAsync(payPeriodId);
            await _paystubEmailHistoryRepository.DeleteByPayPeriodAsync(payPeriodId);
        }

        public async Task DeletePaystubEmailsByEmployeeAndPeriodAsync(int employeeId, int payPeriodId, int organizationId)
        {
            await ValidateIfPayPeriodIsOpenAsync(
                organizationId: organizationId,
                payPeriodId: payPeriodId);

            await _paystubEmailRepository.DeleteByEmployeeAndPayPeriodAsync(
                employeeId: employeeId,
                payPeriodId: payPeriodId);

            await _paystubEmailHistoryRepository.DeleteByEmployeeAndPayPeriodAsync(
                employeeId: employeeId,
                payPeriodId: payPeriodId);
        }

        public async Task UpdateManyThirteenthMonthPaysAsync(ICollection<ThirteenthMonthPay> thirteenthMonthPays)
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

        public async Task UpdateAdjustmentsAsync<T>(int paystubId, ICollection<T> allAdjustments, int modifiedByUserId) where T : IAdjustment
        {
            foreach (var adjustment in allAdjustments)
            {
                if (adjustment.RowID == null || adjustment.RowID <= 0)
                {
                    adjustment.CreatedBy = modifiedByUserId;
                }
                else
                {
                    adjustment.CreatedBy = modifiedByUserId;
                }
            }

            await _paystubRepository.UpdateAdjustmentsAsync(paystubId, allAdjustments);
        }

        private async Task ValidateIfPayPeriodIsOpenAsync(int organizationId, int? payPeriodId)
        {
            var currentOpenPayPeriod = await _payPeriodRepository.GetCurrentOpenAsync(organizationId);

            if (currentOpenPayPeriod == null || currentOpenPayPeriod?.RowID != payPeriodId)
                throw new BusinessLogicException("Only open pay periods can be modified.");
        }
    }
}