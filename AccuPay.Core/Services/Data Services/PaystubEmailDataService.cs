using AccuPay.Core.Entities;
using AccuPay.Core.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Services
{
    public class PaystubEmailDataService : BasePaystubDataService
    {
        private readonly PaystubEmailRepository _paystubEmailRepository;
        private readonly PaystubEmailHistoryRepository _paystubEmailHistoryRepository;

        public PaystubEmailDataService(
            PaystubEmailRepository paystubEmailRepository,
            PaystubEmailHistoryRepository paystubEmailHistoryRepository,
            PayPeriodRepository payPeriodRepository) : base(payPeriodRepository)
        {
            _paystubEmailRepository = paystubEmailRepository;
            _paystubEmailHistoryRepository = paystubEmailHistoryRepository;
        }

        public async Task DeleteByPayPeriodAsync(int payPeriodId, int organizationId)
        {
            await ValidateIfPayPeriodIsOpenAsync(
                organizationId: organizationId,
                payPeriodId: payPeriodId);

            await _paystubEmailRepository.DeleteByPayPeriodAsync(payPeriodId);
            await _paystubEmailHistoryRepository.DeleteByPayPeriodAsync(payPeriodId);
        }

        public async Task DeleteByEmployeeAndPayPeriodAsync(int employeeId, int payPeriodId, int organizationId)
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

        public async Task SetStatusToFailed(int paystubEmailId, string errorLogMessage)
        {
            await _paystubEmailRepository.SetStatusToFailed(paystubEmailId, errorLogMessage);
        }

        public async Task SetStatusToProcessing(int paystubEmailId)
        {
            await _paystubEmailRepository.SetStatusToProcessing(paystubEmailId);
        }

        public async Task Finish(int id, string fileName, string emailAddress)
        {
            await _paystubEmailRepository.Finish(id, fileName, emailAddress);
        }

        public async Task CreateManyAsync(ICollection<PaystubEmail> paystubEmails)
        {
            await _paystubEmailRepository.CreateManyAsync(paystubEmails);
        }

        public async Task ResetAllProcessingAsync()
        {
            await _paystubEmailRepository.ResetAllProcessingAsync();
        }
    }
}