using AccuPay.Core.Entities;
using AccuPay.Core.Enums;
using AccuPay.Core.Helpers;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IPayPeriodDataService
    {
        Task CancelAsync(int payPeriodId, int currentlyLoggedInUserId);

        Task CloseAsync(int payPeriodId, int currentlyLoggedInUserId);

        Task<PayPeriod> CreateAsync(int organizationId, int month, int year, bool isFirstHalf, int currentlyLoggedInUserId);

        Task ReopenAsync(int payPeriodId, int currentlyLoggedInUserId);

        Task<PayPeriod> OpenAsync(int organizationId, int month, int year, bool isFirstHalf, int currentlyLoggedInUserId);

        Task<PayPeriod> OpenAsync(int organizationId, int userId, PayPeriod payPeriod);

        Task<PayPeriod> UpdateStatusAsync(int payPeriodId, int currentlyLoggedInUserId, PayPeriodStatus status);

        Task UpdateStatusAsync(PayPeriod payPeriod, int currentlyLoggedInUserId, PayPeriodStatus status);

        Task<FunctionResult> ValidatePayPeriodActionAsync(int? payPeriodId);
    }
}
