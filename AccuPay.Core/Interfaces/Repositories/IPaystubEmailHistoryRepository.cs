using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IPaystubEmailHistoryRepository
    {
        Task DeleteByEmployeeAndPayPeriodAsync(int employeeId, int payPeriodId);

        Task DeleteByPayPeriodAsync(int payPeriodId);

        Task<IEnumerable<PaystubEmailHistory>> GetByPaystubIdsAsync(int[] paystubIds);
    }
}
