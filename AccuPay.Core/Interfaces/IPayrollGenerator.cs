using AccuPay.Core.Services;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IPayrollGenerator
    {
        Task<PaystubEmployeeResult> Start(
            int employeeId,
            IPayrollResources resources,
            int organizationId,
            int currentlyLoggedInUserId);
    }
}
