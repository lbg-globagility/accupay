using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IPaystubEmailDataService
    {
        Task CreateManyAsync(ICollection<PaystubEmail> paystubEmails);

        Task DeleteByEmployeeAndPayPeriodAsync(int employeeId, int payPeriodId, int organizationId);

        Task DeleteByPayPeriodAsync(int payPeriodId, int organizationId);

        Task Finish(int id, string fileName, string emailAddress);

        Task ResetAllProcessingAsync();

        Task SetStatusToFailed(int paystubEmailId, string errorLogMessage);

        Task SetStatusToProcessing(int paystubEmailId);
    }
}
