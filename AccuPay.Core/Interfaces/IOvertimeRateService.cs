using AccuPay.Core.ValueObjects;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IOvertimeRateService
    {
        Task<OvertimeRate> GetOvertimeRates();
    }
}
