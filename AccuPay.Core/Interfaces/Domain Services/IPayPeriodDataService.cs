using AccuPay.Core.Entities;
using AccuPay.Core.Enums;
using AccuPay.Core.Helpers;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IPayPeriodDataService
    {
        Task CancelAsync(int payPeriodId, int currentlyLoggedInUserId);

        Task CloseAsync(int payPeriodId, int userId);

        Task<PayPeriod> CreateAsync(int organizationId, int month, int year, bool isFirstHalf, int currentUserId);

        Task ReopenAsync(int payPeriodId, int userId);

        Task<PayPeriod> StartStatusAsync(int organizationId, int month, int year, bool isFirstHalf, int currentUserId);

        Task<PayPeriod> UpdateStatusAsync(int payPeriodId, int userId, PayPeriodStatus status);

        Task UpdateStatusAsync(PayPeriod payPeriod, int currentlyLoggedInUserId, PayPeriodStatus status);

        Task<FunctionResult> ValidatePayPeriodActionAsync(int? payPeriodId);
    }
}
