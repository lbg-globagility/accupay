using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IPaystubEmailRepository
    {
        Task CreateManyAsync(ICollection<PaystubEmail> paystubEmails);

        Task DeleteByEmployeeAndPayPeriodAsync(int employeeId, int payPeriodId);

        Task DeleteByPayPeriodAsync(int payPeriodId);

        Task Finish(int id, string fileName, string emailAddress);

        PaystubEmail FirstOnQueueWithPaystubDetails();

        Task<ICollection<PaystubEmail>> GetAllOnQueueAsync();

        Task<ICollection<PaystubEmail>> GetByPaystubIdsAsync(int[] paystubIds);

        Task ResetAllProcessingAsync();

        Task SetStatusToFailed(int paystubEmailId, string errorLogMessage);

        Task SetStatusToProcessing(int paystubEmailId);
    }
}
